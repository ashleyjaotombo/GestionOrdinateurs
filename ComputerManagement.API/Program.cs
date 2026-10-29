using ComputerManagement.API;
using ComputerManagement.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5012", "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IComputerService, ComputerService>();
builder.Services.AddDbContext<ComputerParkContext>(options =>
    options.UseSqlite("Data Source=parcinfo.db"));
builder.Services.AddScoped<IRepairService, RepairService>();
builder.Services.AddSingleton<IStockService, StockService>();
builder.Services.AddSingleton<ILogService, LogService>();

// ajout de swagger JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionOrdinateurs",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez : Bearer {token}"
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement{
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

// Configuration JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Utilisateur utilisant un Token Valide --- {context.Principal?.Identity?.Name ?? "Anonymous"}");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Token Invalide");
                return Task.CompletedTask;
            }
        };
})
// Tokens Keycloak
.AddJwtBearer("Keycloak", options =>
{
    options.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"] ?? "";
    options.RequireHttpsMetadata = false;
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Keycloak:ValidIssuer"],
        ValidateAudience = false,
        NameClaimType = "preferred_username",
        RoleClaimType = "roles"
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            // Le rôle client Keycloak "adminadmin" correspond au rôle "Admin" de l'application
            if (context.Principal?.IsInRole("adminadmin") == true)
                ((ClaimsIdentity)context.Principal.Identity!).AddClaim(new Claim("roles", RoleEnum.Admin.ToString()));
            return Task.CompletedTask;
        }
    };
});

// Autoriser le JWT de AuthentificationApi ou celui de Keycloak
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, "Keycloak")
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();
app.UseCors("AllowBlazorClient");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ComputerParkContext>();
    
    db.Database.Migrate();

    if(!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Login="user1", Role=RoleEnum.Admin },
            new User { Login="user2", Role=RoleEnum.Gestionnaire },
            new User { Login="user3", Role=RoleEnum.Gestionnaire }
        );

        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
