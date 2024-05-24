namespace Sio2324_Grupo_03.Data
{
    public class DataContext : DbContext
    {
        // The sales statistics of the company
        public DbSet<SalesStatistics> SalesStatistics { get; set; }

        // The products of the company
        public DbSet<Product> Products { get; set; }

        // The suppliers of the company
        public DbSet<Supplier> Suppliers { get; set; }

        // The clients of the company
        public DbSet<Client> Clients { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesStatistics>().HasNoKey();
            modelBuilder.Entity<Product>().HasNoKey();
            modelBuilder.Entity<Supplier>().HasNoKey();
            modelBuilder.Entity<Client>().HasNoKey();
        }
    }
}