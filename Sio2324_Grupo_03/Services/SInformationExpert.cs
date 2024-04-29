namespace Sio2324_Grupo_03.Services
{
    public class SInformationExpert : IInformationExpert
    {
        private readonly DataContext _context;

        public SInformationExpert(DataContext context)
        {
            _context = context;
        }

        public SalesStatistics CalculateSalesStatsForDay(int year, int month, int day)
        {
            throw new NotImplementedException();
        }

        public SalesStatistics CalculateSalesStatsForMonth(int year, int month)
        {
            throw new NotImplementedException();
        }

        public SalesStatistics CalculateSalesStatsForQuartile(int year, int quartile)
        {
            SalesStatistics salesStats = new SalesStatistics();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new SqlParameter("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new SqlParameter("@quartile_param", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to SalesStatistics
            string sql = "EXEC CalculateSalesStatsForQuartile @year_param, @quartile_param";
            var result = _context.SalesStatistics.FromSqlRaw(sql, yearParam, quartileParam).FirstOrDefault();

            if (result != null)
            {
                // Map the stored procedure result to the SalesStatistics object
                salesStats.NetAmountEarnedQuartile = result.NetAmountEarnedQuartile;
                salesStats.GrossAmountQuartile = result.GrossAmountQuartile;
                salesStats.QuantitySalesQuartile = result.QuantitySalesQuartile;
            }

            return salesStats;
        }

        public List<TopClient> GetTop3ClientsByValuePerDay(int year, int month, int day)
        {
            throw new NotImplementedException();
        }

        public List<TopClient> GetTop3ClientsByValuePerMonth(int year, int month)
        {
            throw new NotImplementedException();
        }

        public List<TopClient> GetTop3ClientsByValuePerQuartile(int year, int quartile)
        {
            throw new NotImplementedException();
        }

        public List<TopProduct> GetTop3SoldProductsPerDay(int year, int month, int day)
        {
            throw new NotImplementedException();
        }

        // Implementation of GetTop3SoldProductsPerMonth
        public List<TopProduct> GetTop3SoldProductsPerMonth(int year, int month)
        {
            // Implement your logic to fetch top products from the database
            throw new NotImplementedException();
        }

        public List<TopProduct> GetTop3SoldProductsPerQuartile(int year, int quartile)
        {
            throw new NotImplementedException();
        }
        // Other interface methods can be implemented here
    }
}
