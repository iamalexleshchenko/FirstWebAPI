using Microsoft.EntityFrameworkCore;

namespace TestApp1.Database;

public class DatabaseContext : DbContext
{
    // DbSet<T> поля это таблицы в БД
    public DbSet<Product> Products { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public decimal Price { get; set; }
}