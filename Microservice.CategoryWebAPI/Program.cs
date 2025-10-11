using Microservice.CategoryWebAPI.Context;
using Microservice.CategoryWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Steeltoe.Discovery.Consul;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("MyDb"));

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

builder.Services.AddAuthorization();

builder.Services.AddConsulDiscoveryClient();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("categories/hello", (HttpContext httpContext) =>
{
    Console.WriteLine(httpContext.Request.Headers.FirstOrDefault(p => p.Key == "Authorization").Value);
    return new { Message = "Hello Category WebAPI" };
}).RequireAuthorization();

app.MapGet("categories", (ApplicationDbContext dbContext) =>
{
    var res = dbContext.Categories.ToList();
    res.Add(new Category()
    {
        Id = Guid.CreateVersion7(),
        Name = "Meyveler"
    });
    return res;
})
.Produces<List<Category>>();

app.Run();