using Microsoft.EntityFrameworkCore;
using TestApp1.Entities;

namespace TestApp1.Database;

public class DatabaseContext : DbContext
{
    // DbSet<T> поля это таблицы в БД
    public DbSet<Product> Products { get; set; }
    public DbSet<Book> Books { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
}