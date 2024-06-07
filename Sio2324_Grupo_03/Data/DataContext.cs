using Sio2324_Grupo_03.Models;

namespace Sio2324_Grupo_03.Data
{
    public class DataContext : DbContext
    {
        // The sales statistics of the company
        public DbSet<QuartileSalesStatistics> QuartileSalesStatistics { get; set; }

        public DbSet<MonthSalesStatistics> MonthSalesStatistics { get; set; }

        public DbSet<DailySalesStatistics> DailySalesStatistics { get; set; }

        public DbSet<QuartileAverageDailySales> AverageDailySales { get; set; }

        public DbSet<QuartileMonthSalesMode> MonthSalesModes { get; set; }

        public DbSet<QuartileProductMovementsStatistics> QuartileProductMovementsStatistics { get; set; }

        public DbSet<MonthProductMovementsStatistics> MonthProductMovementsStatistics { get; set; }

        // The products of the company
        public DbSet<QuartileProductStatistics> QuartileProductStatistics { get; set; }

        public DbSet<MonthProductStatistics> MonthProductStatistics { get; set; }

        public DbSet<DailyProductStatistics> DailyProductStatistics { get; set; }

        // The suppliers of the company
        public DbSet<QuartileSupplierStatistics> QuartileSupplierStatistics { get; set; }

        public DbSet<MonthSupplierStatistics> MonthSupplierStatistics { get; set; }

        public DbSet<DailySupplierStatistics> DailySupplierStatistics { get; set; }

        public DbSet<QuartilePurchasesFromSuppliers> QuartilePurchasesFromSuppliers { get; set; }

        // The clients of the company
        public DbSet<QuartileClientStatistics> QuartileClientsStatistics { get; set; }

        public DbSet<MonthClientStatistics> MonthClientsStatistics { get; set; }

        public DbSet<DailyClientStatistics> DailyClientsStatistics { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuartileSalesStatistics>().HasNoKey();
            modelBuilder.Entity<MonthSalesStatistics>().HasNoKey();
            modelBuilder.Entity<DailySalesStatistics>().HasNoKey();
            modelBuilder.Entity<QuartileAverageDailySales>().HasNoKey();
            modelBuilder.Entity<QuartileMonthSalesMode>().HasNoKey();
            modelBuilder.Entity<QuartileProductMovementsStatistics>().HasNoKey();
            modelBuilder.Entity<MonthProductMovementsStatistics>().HasNoKey();

            modelBuilder.Entity<QuartileProductStatistics>().HasNoKey();
            modelBuilder.Entity<MonthProductStatistics>().HasNoKey();
            modelBuilder.Entity<DailyProductStatistics>().HasNoKey();

            modelBuilder.Entity<QuartileSupplierStatistics>().HasNoKey();
            modelBuilder.Entity<MonthSupplierStatistics>().HasNoKey();
            modelBuilder.Entity<DailySupplierStatistics>().HasNoKey();
            modelBuilder.Entity<QuartilePurchasesFromSuppliers>().HasNoKey();

            modelBuilder.Entity<QuartileClientStatistics>().HasNoKey();
            modelBuilder.Entity<MonthClientStatistics>().HasNoKey();
            modelBuilder.Entity<DailyClientStatistics>().HasNoKey();
        }
    }
}