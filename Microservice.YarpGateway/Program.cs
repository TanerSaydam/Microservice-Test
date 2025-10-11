using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("yarp.json", optional: false);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication()
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Issuer",
            ValidAudience = "Audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my secret key,my secret key,my secret keymy secret keymy secret keymy secret keymy secret keymy secret keymy secret key"))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiPolicy", p => p.RequireAuthenticatedUser());
    options.AddPolicy("AdminOnly", p => p.RequireRole("admin"));
});

builder.Services.AddRateLimiter(opt =>
{
    opt.AddFixedWindowLimiter("fixed", cfr =>
    {
        cfr.QueueLimit = 1;
        cfr.PermitLimit = 5;
        cfr.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        cfr.Window = TimeSpan.FromMinutes(1);
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("login", () =>
{
    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my secret key,my secret key,my secret keymy secret keymy secret keymy secret keymy secret keymy secret keymy secret key"));
    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);
    JwtSecurityToken securityToken = new(
        issuer: "Issuer",
        audience: "Audience",
        claims: new List<Claim>() { new Claim("UserId", Guid.CreateVersion7().ToString()) },
        notBefore: DateTime.Now,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: signinCredentials
        );

    var handler = new JwtSecurityTokenHandler();
    var token = handler.WriteToken(securityToken);

    return new { Token = token };
});

app.MapGet("/", () => "Hello World!");

app.UseRateLimiter();

app.MapReverseProxy();

app.Run();

//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/config-files?view=aspnetcore-9.0