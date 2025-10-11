using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json");

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

builder.Services.AddOcelot(builder.Configuration).AddPolly();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

await app.UseOcelot();

//await app.UseOcelot(new OcelotPipelineConfiguration
//{
//    AuthenticationMiddleware = async (context, next) =>
//    {
//        await next.Invoke();
//    }
//});

app.MapGet("/", () => "Hello World!");

await app.RunAsync();

//https://ocelot.readthedocs.io/en/latest/features/configuration.html