namespace Microservice.ProductWebAPI.Models;

public sealed class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}