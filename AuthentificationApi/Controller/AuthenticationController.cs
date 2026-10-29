namespace AuthentificationApi;

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using ComputerManagement.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IConfiguration configuration, IHttpClientFactory factory) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpClientFactory _factory = factory;


    [HttpPost("login")]
    public async Task<IActionResult> Login(String login)
    {
        var client = _factory.CreateClient("HttpClientName");
        User? user;
        try
        {
            user = await client.GetFromJsonAsync<User>($"users/userbylogin/{login}");
        }
        catch (HttpRequestException)
        {
            // return 401 if the user is not found
            return Unauthorized();
        }

        if (user == null)
            return Unauthorized();

        var jwtKey = _configuration["Jwt:Key"] ?? "";
        var key = Encoding.ASCII.GetBytes(jwtKey);

        return Ok(GenTokenJWT(key, user.Role, user.Login));
    }


    private string GenTokenJWT(byte[] keyP, RoleEnum role, String login)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new[] {
            new Claim(ClaimTypes.Name, login),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyP), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}
