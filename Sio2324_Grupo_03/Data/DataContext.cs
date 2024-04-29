namespace Sio2324_Grupo_03.Data
{
    public class DataContext : DbContext
    {
        public DbSet<SalesStatistics> SalesStatistics { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
