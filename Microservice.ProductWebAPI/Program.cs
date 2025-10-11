using Microservice.ProductWebAPI.Context;
using Microservice.ProductWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Registry;
using Polly.Retry;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("MyDb"));

builder.Services.AddHealthChecks();

builder.Services.AddConsulDiscoveryClient();

builder.Services.AddResiliencePipeline("my-pipeline", builder =>
{
    builder
    .AddRetry(new RetryStrategyOptions()
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(10), //İlk başarısızlıktan sonra bir sonraki denemeye kadar 10 saniye bekle anlamına gelir.
        BackoffType = DelayBackoffType.Exponential, //Bu, bekleme süresinin her denemede katlanarak artmasını sağlar.
        UseJitter = true, //Bu, çok sayıda istek aynı anda yeniden deneme yapıyorsa (örneğin 100 mikroservis aynı anda Consul’a istek atıyorsa) hepsinin aynı anda tekrar denememesini sağlar.
        ShouldHandle = new PredicateBuilder() //Bu, hangi hatalarda yeniden deneneceğini belirler.
            .Handle<Exception>() // Consul erişim hataları dâhil
    })
    .AddTimeout(TimeSpan.FromSeconds(30)); // 30 sn içinde tamamlanmazsa iptal et
});

builder.Services.AddHttpClient();

//🧩 DelayBackoffType Enum Türleri
//Değer	Açıklama	Örnek Davranış
//Constant	Her denemede sabit süre bekler.	Delay = 5s → 5s, 5s, 5s
//Linear	Her denemede gecikme lineer (doğrusal) artar.	Delay = 5s → 5s, 10s, 15s
//Exponential	Her denemede gecikme katlanarak (üstel) artar.	Delay = 5s → 5s, 10s, 20s, 40s

var app = builder.Build();

app.MapGet("test", () =>
{
    Console.WriteLine("I am working...");
    return new { Message = "Product WebAPI" };
});

app.MapGet(string.Empty, async (ApplicationDbContext dbContext, IDiscoveryClient discoveryClient, CancellationToken cancellationToken, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipelineProvider) =>
{
    var pipeline = resiliencePipelineProvider.GetPipeline("my-pipeline");
    var categoryServices =
    await pipeline.ExecuteAsync(async callback => await discoveryClient.GetInstancesAsync("CategoryWebAPI", default));

    if (!categoryServices.Any())
    {
        return Results.NotFound(categoryServices);
    }

    var uri = categoryServices.First().Uri;
    httpClient.BaseAddress = uri;
    var categories = await pipeline.ExecuteAsync(async callback => await httpClient.GetFromJsonAsync<List<Category>>("categories", cancellationToken));
    var res = dbContext.Products.ToList();
    res.Add(new Product()
    {
        Name = "Elma",
        Categories = categories ?? []
    });
    return Results.Ok(res);
})
.Produces<List<Product>>();

app.MapHealthChecks("/health");

app.Run();