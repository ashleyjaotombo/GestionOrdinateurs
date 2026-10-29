namespace AuthentificationApi;

using Microsoft.AspNetCore.Mvc;

// Login via Keycloak
[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration configuration, IHttpClientFactory factory) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpClientFactory _factory = factory;


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var client = _factory.CreateClient("Keycloak");
        var realm = _configuration["Keycloak:Realm"];

        // "Direct access grants" : Keycloak vérifie user / mot de passe et renvoie les tokens
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = _configuration["Keycloak:ClientId"] ?? "",
            ["client_secret"] = _configuration["Keycloak:ClientSecret"] ?? "",
            ["username"] = request.Username,
            ["password"] = request.Password
        });

        HttpResponseMessage response;
        try
        {
            response = await client.PostAsync($"realms/{realm}/protocol/openid-connect/token", form);
        }
        catch (HttpRequestException)
        {
            // Keycloak injoignable
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        // retourner 401 si user ou mot de passe incorrect
        if (!response.IsSuccessStatusCode)
            return Unauthorized();

        // envoi de access_token, expires_in, refresh_token
        var json = await response.Content.ReadAsStringAsync();
        return Content(json, "application/json");
    }
}

public record LoginRequest(string Username, string Password);
