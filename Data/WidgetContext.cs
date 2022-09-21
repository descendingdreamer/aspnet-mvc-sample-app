using Microsoft.EntityFrameworkCore;
using WebForm.Models;   

namespace WebForm.Data;

public class WidgetContext : DbContext
{
    public WidgetContext(DbContextOptions<WidgetContext> options) : base(options)
    {
    }
    
    public DbSet<Widget>? Widgets { get; set; }
    public DbSet<WidgetType>? Types { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Widget>().ToTable("Widgets");
        modelBuilder.Entity<WidgetType>().ToTable("Types");
    }
}