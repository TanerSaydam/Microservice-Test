using Microservice.CategoryWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservice.CategoryWebAPI.Context;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
}