namespace Sio2324_Grupo_03.Services
{
    public class SInformationExpert : IInformationExpert
    {
        private readonly DataContext _context;

        public SInformationExpert(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Calculate the sales statistics for a day
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <param name="day">Day of the sales</param>
        /// <returns>The sales statistics for a specific time period</returns>
        public object CalculateSalesStatsForDay([FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            SalesStatistics salesStatistics = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };
            SqlParameter dayParam = new("@day_param", SqlDbType.Int) { Value = day };

            // Execute the stored procedure and map the results to Object
            string sql = "EXEC CalculateSalesStatsForDay @year_param, @month_param, @day_param";
            var result = _context.SalesStatistics.FromSqlRaw(sql, yearParam, monthParam, dayParam).First();

            if (result != null)
            {
                // Map the stored procedure result to the Object object
                salesStatistics.NetAmountEarned = result.NetAmountEarned;
                salesStatistics.GrossAmountEarned = result.GrossAmountEarned;
                salesStatistics.QuantitySales = result.QuantitySales;
            }

            return salesStatistics;
        }

        /// <summary>
        /// Calculate the sales statistics for a month
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <returns>The sales statistics for a specific time period</returns>
        public object CalculateSalesStatsForMonth([FromQuery] int year, [FromQuery] int month)
        {
            SalesStatistics salesStats = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };

            // Execute the stored procedure and map the results to Object
            string sql = "EXEC CalculateSalesStatsForMonth @year_param, @month_param";
            var result = _context.SalesStatistics.FromSqlRaw(sql, yearParam, monthParam).First();

            if (result != null)
            {
                // Map the stored procedure result to the Object object
                salesStats.NetAmountEarned = result.NetAmountEarned;
                salesStats.GrossAmountEarned = result.GrossAmountEarned;
                salesStats.QuantitySales = result.QuantitySales;
            }

            return salesStats;
        }

        /// <summary>
        /// Calculate the sales statistics for a quartile
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="quartile">Quartile of the sales</param>
        /// <returns>The sales statistics for a specific time period</returns>
        public object CalculateSalesStatsForQuartile([FromQuery] int year, [FromQuery] int quartile)
        {
            SalesStatistics salesStats = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile_param", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to Object
            string sql = "EXEC CalculateSalesStatsForQuartile @year_param, @quartile_param";
            var result = _context.SalesStatistics.FromSqlRaw(sql, yearParam, quartileParam).First();

            if (result != null)
            {
                // Map the stored procedure result to the Object object
                salesStats.NetAmountEarned = result.NetAmountEarned;
                salesStats.GrossAmountEarned = result.GrossAmountEarned;
                salesStats.QuantitySales = result.QuantitySales;
            }

            return salesStats;
        }

        /// <summary>
        /// Get the top 3 clients by value per day
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <param name="day">Day of the sales</param>
        /// <returns>A list of the top 3 clients for a specific time period</returns>
        public object GetTop3ClientsByValuePerDay([FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            List<Client> clients = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month", SqlDbType.Int) { Value = month };
            SqlParameter dayParam = new("@day", SqlDbType.Int) { Value = day };

            // Execute the stored procedure and map the results to Client
            string sqlCommand = "EXEC [dbo].[GetTop3ClientsByValuePerDay] @year_param, @month_param, @day_param";
            var results = _context.Clients.FromSqlRaw(sqlCommand, yearParam, monthParam, dayParam).ToList();

            // Map the stored procedure results to the Client objects
            foreach (var result in results)
            {
                clients.Add(new Client
                {
                    Year = result.Year,
                    Month = result.Month,
                    Day = result.Day,
                    ClientName = result.ClientName,
                    SpentMoney = result.SpentMoney
                });
            }

            // Create a new object that includes the caption and the clients
            var clientsReturned = new
            {
                Caption = "Top 3 clients by value per day",
                Clients = clients
            };

            return clientsReturned;
        }

        /// <summary>
        /// Get the top 3 clients by value per month
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <returns>A list of the top 3 clients for a specific time period</returns>
        public object GetTop3ClientsByValuePerMonth([FromQuery] int year, [FromQuery] int month)
        {
            List<Client> clients = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month", SqlDbType.Int) { Value = month };

            // Execute the stored procedure and map the results to Client
            string sqlCommand = "EXEC [dbo].[GetTop3ClientsByValuePerMonth] @year_param, @month_param";
            var results = _context.Clients.FromSqlRaw(sqlCommand, yearParam, monthParam).ToList();

            // Map the stored procedure results to the Client objects
            foreach (var result in results)
            {
                clients.Add(new Client
                {
                    Year = result.Year,
                    Month = result.Month,
                    ClientName = result.ClientName,
                    SpentMoney = result.SpentMoney
                });
            }

            // Create a new object that includes the caption and the clients
            var clientsReturned = new
            {
                Caption = "Top 3 clients by value per month",
                Clients = clients
            };

            return clientsReturned;
        }

        /// <summary>
        /// Get the top 3 clients by value per quartile
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="quartile">Quartile of the sales</param>
        /// <returns>A list of the top 3 clients for a specific time period</returns>
        public object GetTop3ClientsByValuePerQuartile([FromQuery] int year, [FromQuery] int quartile)
        {
            List<Client> clients = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to Client
            string sqlCommand = "EXEC [dbo].[GetTop3ClientsByValuePerQuartile] @year_param, @quartile_param";
            var results = _context.Clients.FromSqlRaw(sqlCommand, yearParam, quartileParam).ToList();

            // Map the stored procedure results to the Client objects
            foreach (var result in results)
            {
                clients.Add(new Client
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    ClientName = result.ClientName,
                    SpentMoney = result.SpentMoney
                });
            }

            // Create a new object that includes the caption and the clients
            var clientsReturned = new
            {
                Caption = "Top 3 clients by value per quartile",
                Clients = clients
            };

            return clientsReturned;
        }

        /// <summary>
        /// Get the top 3 sold products per day
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <param name="day">Day of the sales</param>
        /// <returns>A list of the top 3 sold products for a specific time period</returns>
        public object GetTop3SoldProductsPerDay([FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            List<Product> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month", SqlDbType.Int) { Value = month };
            SqlParameter dayParam = new("@day", SqlDbType.Int) { Value = day };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3SoldProductsPerDay] @year_param, @month_param, @day_param";
            var results = _context.Products.FromSqlRaw(sqlCommand, yearParam, monthParam, dayParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new Product
                {
                    Year = result.Year,
                    Month = result.Month,
                    Day = result.Day,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Create a new object that includes the caption and the products
            var productsReturned = new
            {
                Caption = "Top 3 sold products per day",
                Products = products
            };

            return productsReturned;
        }

        /// <summary>
        /// Get the top 3 sold products per month
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <returns>The list of the top 3 sold products for a specific time period</returns>
        public object GetTop3SoldProductsPerMonth([FromQuery] int year, [FromQuery] int month)
        {
            List<Product> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month", SqlDbType.Int) { Value = month };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3SoldProductsPerMonth] @year_param, @month_param";
            var results = _context.Products.FromSqlRaw(sqlCommand, yearParam, monthParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new Product
                {
                    Year = result.Year,
                    Month = result.Month,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Create a new object that includes the caption and the products
            var productsReturned = new
            {
                Caption = "Top 3 sold products per month",
                Products = products
            };

            return productsReturned;
        }

        /// <summary>
        /// Get the top 3 sold products per quartile
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="quartile">Quartile of the sales</param>
        /// <returns>The list of the top 3 sold products for a specific time period</returns>
        public object GetTop3SoldProductsPerQuartile([FromQuery] int year, [FromQuery] int quartile)
        {
            List<Product> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3SoldProductsPerQuartile] @year_param, @quartile_param";
            var results = _context.Products.FromSqlRaw(sqlCommand, yearParam, quartileParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new Product
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Create a new object that includes the caption and the products
            var productsReturned = new
            {
                Caption = "Top 3 sold products per quartile",
                Products = products
            };

            return productsReturned;
        }

        /// <summary>
        /// Function to get the purchases from suppliers per quartile
        /// </summary>
        /// <returns>The list of the purchases from suppliers per quartile</returns>
        public object GetPurchasesFromSuppliersPerQuartile()
        {
            List<Supplier> purchasesFromSuppliers = new();

            // Execute the stored procedure and map the results to Supplier
            string sqlCommand = "EXEC [dbo].[PurchasesFromSuppliersPerQuartile]";
            var results = _context.Suppliers.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the Supplier objects
            foreach (var result in results)
            {
                purchasesFromSuppliers.Add(new Supplier
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    SupplierName = result.SupplierName,
                    SpentMoney = result.SpentMoney
                });
            }

            // Create a new object that includes the caption and the suppliers
            var purchasesFromSuppliersReturned = new
            {
                Caption = "List of purchases from suppliers per quartile",
                Suppliers = purchasesFromSuppliers
            };

            return purchasesFromSuppliersReturned;
        }

        /// <summary>
        /// Function to get the most sold products per quartile
        /// </summary>
        /// <returns>The list of the most sold products per quartile</returns>
        public object GetMostSoldProductsPerQuartile()
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
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Create a new object that includes the caption and the suppliers
            var mostSoldProductsReturned = new
            {
                Caption = "List of the most sold products per quartile",
                Suppliers = mostSoldProducts
            };

            return mostSoldProductsReturned;
        }
    }
}