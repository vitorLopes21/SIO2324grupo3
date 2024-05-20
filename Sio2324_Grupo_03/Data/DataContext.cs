namespace Sio2324_Grupo_03.Data
{
    public class DataContext : DbContext
    {
        public DbSet<SalesStatistics> SalesStatistics { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Suppliers> Suppliers { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesStatistics>().HasNoKey();
            modelBuilder.Entity<Product>().HasNoKey();
            modelBuilder.Entity<Suppliers>().HasNoKey();
            modelBuilder.Entity<Client>().HasNoKey();
        }
    }
}