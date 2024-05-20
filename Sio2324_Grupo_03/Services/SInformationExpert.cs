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

        public List<Client> GetTop3ClientsByValuePerDay(int year, int month, int day)
        {
            throw new NotImplementedException();
        }

        public List<Client> GetTop3ClientsByValuePerMonth(int year, int month)
        {
            throw new NotImplementedException();
        }

        public List<Client> GetTop3ClientsByValuePerQuartile(int year, int quartile)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetTop3SoldProductsPerDay(int year, int month, int day)
        {
            throw new NotImplementedException();
        }

        // Implementation of GetTop3SoldProductsPerMonth
        public List<Product> GetTop3SoldProductsPerMonth(int year, int month)
        {
            // Implement your logic to fetch top products from the database
            throw new NotImplementedException();
        }

        public List<Product> GetTop3SoldProductsPerQuartile(int year, int quartile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function to get the purchases from suppliers per quartile
        /// </summary>
        /// <returns>The list of the purchases from suppliers per quartile</returns>
        public List<Suppliers> GetPurchasesFromSuppliersPerQuartile()
        {
            List<Suppliers> purchasesFromSuppliers = new();

            // Execute the stored procedure and map the results to Supplier
            string sqlCommand = "EXEC [dbo].[PurchasesFromSuppliersPerQuartile]";
            var results = _context.Suppliers.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the Supplier objects
            foreach (var result in results)
            {
                purchasesFromSuppliers.Add(new Suppliers
                {
                    Quartile = result.Quartile,
                    Supplier = result.Supplier,
                    SpentMoney = result.SpentMoney
                });
            }

            return purchasesFromSuppliers;
        }

        /// <summary>
        /// Function to get the most sold products per quartile
        /// </summary>
        /// <returns>The list of the most sold products per quartile</returns>
        public List<Product> GetMostSoldProductsPerQuartile()
        {
            // Create a list to store the most sold products
            List<Product> mostSoldProducts = new();

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[MostSoldProductsByQuartile]";
            var results = _context.Products.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the TopProduct objects
            foreach (var result in results)
            {
                mostSoldProducts.Add(new Product
                {
                    Quartile = result.Quartile,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            return mostSoldProducts;
        }


        /// <summary>
        /// Function to get the average daily sales per quartile
        /// </summary>
        /// <returns>The list of the average daily sales per quartile</returns>
        public List<Sales> GetAVGDailySalesPerQuartile()
        {
            // Create a list to store the average daily sales
            List<Sales> AVGDailySales = new();

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[AverageDailySalesByQuartile]";
            var results = _context.Sales.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the TopProduct objects
            foreach (var result in results)
            {
                AVGDailySales.Add(new Sales
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Money = result.Money
                });
            }

            return AVGDailySales;
        }
    }
}