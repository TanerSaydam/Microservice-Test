namespace Microservice.ProductWebAPI.Models;

public sealed class Product
{
    public Product()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<Category> Categories { get; set; } = new();
}