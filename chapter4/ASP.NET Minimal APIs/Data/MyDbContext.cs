namespace MyMicroservice.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
}
