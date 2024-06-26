﻿using System;

namespace Sio2324_Grupo_03.Services
{
    public class SInformationExpert : IInformationExpert
    {
        private readonly DataContext _context;
        private readonly ILogger<SInformationExpert> _logger;

        public SInformationExpert(DataContext context, ILogger<SInformationExpert> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Calculate the sales statistics for a day
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <param name="day">Day of the sales</param>
        /// <returns>The sales statistics for a specific time period</returns>
        public object CalculateSalesStatsForDay(int year, int month, int day)
        {
            List<DailySalesStatistics> salesStatistics = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };
            SqlParameter dayParam = new("@day_param", SqlDbType.Int) { Value = day };

            // Execute the stored procedure and map the results to Object
            string sql = "EXEC [dbo].[CalculateSalesStatsForDay] @year_param, @month_param, @day_param";
            var results = _context.DailySalesStatistics.FromSqlRaw(sql, yearParam, monthParam, dayParam).ToList();

            foreach (var result in results)
            {
                // Map the stored procedure result to the Object object
                salesStatistics.Add(new DailySalesStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Day = result.Day,
                    Company = result.Company,
                    NetAmountEarned = result.NetAmountEarned,
                    GrossAmountEarned = result.GrossAmountEarned,
                    QuantitySold = result.QuantitySold
                });
            }

            // Create a new object that includes the caption and the sales statistics
            var salesStatsReturned = new
            {
                Caption = "Sales statistics for a day",
                SalesStatistics = salesStatistics
            };

            return salesStatsReturned;
        }

        /// <summary>
        /// Calculate the sales statistics for a month
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <returns>The sales statistics for a specific time period</returns>
        public object CalculateSalesStatsForMonth(int year, int month)
        {
            List<MonthSalesStatistics> salesStats = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };

            // Execute the stored procedure and map the results to Object
            string sql = "EXEC [dbo].[CalculateSalesStatsForMonth] @year_param, @month_param";
            var results = _context.MonthSalesStatistics.FromSqlRaw(sql, yearParam, monthParam).ToList();

            foreach (var result in results)
            {
                // Map the stored procedure result to the Object object
                salesStats.Add(new MonthSalesStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Company = result.Company,
                    NetAmountEarned = result.NetAmountEarned,
                    GrossAmountEarned = result.GrossAmountEarned,
                    QuantitySold = result.QuantitySold
                });
            }

            // Create a new object that includes the caption and the sales statistics
            var salesStatsReturned = new
            {
                Caption = "Sales statistics for a month",
                SalesStatistics = salesStats
            };

            return salesStatsReturned;
        }

        /// <summary>
        /// Calculate the sales statistics for a quartile
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="quartile">Quartile of the sales</param>
        /// <returns>The sales statistics for a specific time period</returns>
        public object CalculateSalesStatsForQuartile(int year, int quartile)
        {
            List<QuartileSalesStatistics> salesStats = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile_param", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to Object
            string sql = "EXEC [dbo].[CalculateSalesStatsForQuartile] @year_param, @quartile_param";
            var results = _context.QuartileSalesStatistics.FromSqlRaw(sql, yearParam, quartileParam).ToList();

            foreach (var result in results)
            {
                // Map the stored procedure result to the Object object
                salesStats.Add(new QuartileSalesStatistics
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    NetAmountEarned = result.NetAmountEarned,
                    GrossAmountEarned = result.GrossAmountEarned,
                    QuantitySold = result.QuantitySold
                });
            }

            // Create a new object that includes the caption and the sales statistics
            var salesStatsReturned = new
            {
                Caption = "Sales statistics for a quartile",
                SalesStatistics = salesStats
            };

            return salesStatsReturned;
        }

        public object CalculateProductStatsForMonth()
        {
            List<MonthProductMovementsStatistics> productMovementStats = new();

           

            // Execute the stored procedure and map the results to Object
            string sql = "EXEC [dbo].[CalculateProductMovementsForMonth]";
            var results = _context.MonthProductMovementsStatistics.FromSqlRaw(sql).ToList();

            foreach (var result in results)
            {
                // Map the stored procedure result to the Object object
                productMovementStats.Add(new MonthProductMovementsStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Company = result.Company,
                    Description = result.Description,
                    EntryQuantity = result.EntryQuantity,
                    ExitQuantity = result.ExitQuantity
                });
            }

            // Create a new object that includes the caption and the sales statistics
            var productMovementStatsReturned = new
            {
                Caption = "Product Movement statistics for a month",
                ProductMovement = productMovementStats
            };

            return productMovementStatsReturned;
        }

        public object CalculateProductStatsForQuartile()
        {
            List<QuartileProductMovementsStatistics> productMovementStats = new();

            // Execute the stored procedure and map the results to Object
            string sql = "EXEC [dbo].[CalculateProductMovementsForQuartile]";
            var results = _context.QuartileProductMovementsStatistics.FromSqlRaw(sql).ToList();

            foreach (var result in results)
            {
                // Map the stored procedure result to the Object object
                productMovementStats.Add(new QuartileProductMovementsStatistics
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    Description = result.Description,
                    EntryQuantity = result.EntryQuantity,
                    ExitQuantity = result.ExitQuantity
                });
            }

            // Create a new object that includes the caption and the sales statistics
            var productMovementStatsReturned = new
            {
                Caption = "Product Movement statistics for a quartile",
                ProductMovement = productMovementStats
            };

            return productMovementStatsReturned;
        }   

        /// <summary>
        /// Get the top 3 clients by value per day
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <param name="day">Day of the sales</param>
        /// <returns>A list of the top 3 clients for a specific time period</returns>
        public object GetTop3ClientsByValuePerDay(int year, int month, int day)
        {
            List<DailyClientStatistics> clients = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };
            SqlParameter dayParam = new("@day_param", SqlDbType.Int) { Value = day };

            // Execute the stored procedure and map the results to Client
            string sqlCommand = "EXEC [dbo].[GetTop3ClientsByValuePerDay] @year_param, @month_param, @day_param";
            var results = _context.DailyClientsStatistics.FromSqlRaw(sqlCommand, yearParam, monthParam, dayParam).ToList();

            // Map the stored procedure results to the Client objects
            foreach (var result in results)
            {
                clients.Add(new DailyClientStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Day = result.Day,
                    Company = result.Company,
                    ClientName = result.ClientName,
                    SpentMoney = result.SpentMoney,
                    BoughtProducts = result.BoughtProducts
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
        public object GetTop3ClientsByValuePerMonth(int year, int month)
        {
            List<MonthClientStatistics> clients = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };

            // Execute the stored procedure and map the results to Client
            string sqlCommand = "EXEC [dbo].[GetTop3ClientsByValuePerMonth] @year_param, @month_param";
            var results = _context.MonthClientsStatistics.FromSqlRaw(sqlCommand, yearParam, monthParam).ToList();

            // Map the stored procedure results to the Client objects
            foreach (var result in results)
            {
                clients.Add(new MonthClientStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Company = result.Company,
                    ClientName = result.ClientName,
                    SpentMoney = result.SpentMoney,
                    BoughtProducts = result.BoughtProducts
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
        public object GetTop3ClientsByValuePerQuartile(int year, int quartile)
        {
            List<QuartileClientStatistics> clients = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile_param", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to Client
            string sqlCommand = "EXEC [dbo].[GetTop3ClientsByValuePerQuartile] @year_param, @quartile_param";
            var results = _context.QuartileClientsStatistics.FromSqlRaw(sqlCommand, yearParam, quartileParam).ToList();

            // Map the stored procedure results to the Client objects
            foreach (var result in results)
            {
                clients.Add(new QuartileClientStatistics
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    ClientName = result.ClientName,
                    SpentMoney = result.SpentMoney,
                    BoughtProducts = result.BoughtProducts
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
        /// Get the top 3 suppliers by value per quartile
        /// </summary>
        /// <param name="year">Year of the purchases</param>
        /// <param name="month">Month of the purchases</param>
        /// <param name="day">Day of the purchases</param>
        /// <returns>A list of the top 3 suppliers for a specific time period</returns>
        public object GetTop3SuppliersByValuePerDay(int year, int month, int day)
        {
            List<DailySupplierStatistics> suppliers = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };
            SqlParameter dayParam = new("@day_param", SqlDbType.Int) { Value = day };

            // Execute the stored procedure and map the results to Supplier
            string sqlCommand = "EXEC [dbo].[GetTop3SuppliersByValuePerDay] @year_param, @month_param, @day_param";
            var results = _context.DailySupplierStatistics.FromSqlRaw(sqlCommand, yearParam, monthParam, dayParam).ToList();

            // Map the stored procedure results to the Supplier objects
            foreach (var result in results)
            {
                suppliers.Add(new DailySupplierStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Day = result.Day,
                    Company = result.Company,
                    SupplierName = result.SupplierName,
                    City = result.City,
                    Market = result.Market,
                    SpentMoney = result.SpentMoney,
                    BoughtProducts = result.BoughtProducts
                });
            }

            // Create a new object that includes the caption and the suppliers
            var suppliersReturned = new
            {
                Caption = "Top 3 suppliers by value per day",
                Suppliers = suppliers
            };

            return suppliersReturned;
        }

        /// <summary>
        /// Get the top 3 suppliers by value per month
        /// </summary>
        /// <param name="year">Year of the purchases</param>
        /// <param name="month">Month of the purchases</param>
        /// <returns>A list of the top 3 suppliers for a specific time period</returns>
        public object GetTop3SuppliersByValuePerMonth(int year, int month)
        {
            List<MonthSupplierStatistics> suppliers = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };

            // Execute the stored procedure and map the results to Supplier
            string sqlCommand = "EXEC [dbo].[GetTop3SuppliersByValuePerMonth] @year_param, @month_param";
            var results = _context.MonthSupplierStatistics.FromSqlRaw(sqlCommand, yearParam, monthParam).ToList();

            // Map the stored procedure results to the Supplier objects
            foreach (var result in results)
            {
                suppliers.Add(new MonthSupplierStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Company = result.Company,
                    SupplierName = result.SupplierName,
                    City = result.City,
                    Market = result.Market,
                    SpentMoney = result.SpentMoney,
                    BoughtProducts = result.BoughtProducts
                });
            }

            // Create a new object that includes the caption and the suppliers
            var suppliersReturned = new
            {
                Caption = "Top 3 suppliers by value per month",
                Suppliers = suppliers
            };

            return suppliersReturned;
        }

        /// <summary>
        /// Get the top 3 suppliers by value per quartile
        /// </summary>
        /// <param name="year">Year of the purchases</param>
        /// <param name="quartile">Month of the purchases</param>
        /// <returns>A list of the top 3 suppliers for a specific time period</returns>
        public object GetTop3SuppliersByValuePerQuartile(int year, int quartile)
        {
            List<QuartileSupplierStatistics> suppliers = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile_param", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to Supplier
            string sqlCommand = "EXEC [dbo].[GetTop3SuppliersByValuePerQuartile] @year_param, @quartile_param";
            var results = _context.QuartileSupplierStatistics.FromSqlRaw(sqlCommand, yearParam, quartileParam).ToList();

            // Map the stored procedure results to the Supplier objects
            foreach (var result in results)
            {
                suppliers.Add(new QuartileSupplierStatistics
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    SupplierName = result.SupplierName,
                    City = result.City,
                    Market = result.Market,
                    SpentMoney = result.SpentMoney,
                    BoughtProducts = result.BoughtProducts
                });
            }

            // Create a new object that includes the caption and the suppliers
            var suppliersReturned = new
            {
                Caption = "Top 3 suppliers by value per quartile",
                Suppliers = suppliers
            };

            return suppliersReturned;
        }

        /// <summary>
        /// Get the top 3 sold products per day
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <param name="day">Day of the sales</param>
        /// <returns>A list of the top 3 sold products for a specific time period</returns>
        public object GetTop3MostSoldProductsPerDay(int year, int month, int day)
        {
            List<DailyProductStatistics> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };
            SqlParameter dayParam = new("@day_param", SqlDbType.Int) { Value = day };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3MostSoldProductsPerDay] @year_param, @month_param, @day_param";
            var results = _context.DailyProductStatistics.FromSqlRaw(sqlCommand, yearParam, monthParam, dayParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new DailyProductStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Day = result.Day,
                    Company = result.Company,
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
        /// <returns>A list of the top 3 sold products for a specific time period</returns>
        public object GetTop3MostSoldProductsPerMonth(int year, int month)
        {
            List<MonthProductStatistics> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3MostSoldProductsPerMonth] @year_param, @month_param";
            var results = _context.MonthProductStatistics.FromSqlRaw(sqlCommand, yearParam, monthParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new MonthProductStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Company = result.Company,
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
        /// <returns>A list of the top 3 sold products for a specific time period</returns>
        public object GetTop3MostSoldProductsPerQuartile(int year, int quartile)
        {
            List<QuartileProductStatistics> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile_param", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3MostSoldProductsPerQuartile] @year_param, @quartile_param";
            var results = _context.QuartileProductStatistics.FromSqlRaw(sqlCommand, yearParam, quartileParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new QuartileProductStatistics
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
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
        /// Get the top 3 least sold products per day
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <param name="day">Day of the sales</param>
        /// <returns>A list of the top 3 least sold products for a specific time period</returns>
        public object GetTop3LeastSoldProductsPerDay(int year, int month, int day)
        {
            List<DailyProductStatistics> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };
            SqlParameter dayParam = new("@day_param", SqlDbType.Int) { Value = day };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3LeastSoldProductsPerDay] @year_param, @month_param, @day_param";
            var results = _context.DailyProductStatistics.FromSqlRaw(sqlCommand, yearParam, monthParam, dayParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new DailyProductStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Day = result.Day,
                    Company = result.Company,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Order the products by MoneyEarnedFromSales in ascending order to get the least sold products
            var sortedProducts = products.OrderBy(p => p.MoneyEarnedFromSales).ToList();

            // Take the top 3 least sold products
            var top3LeastSoldProducts = sortedProducts.Take(3);

            // Create a new object that includes the caption and the products
            var productsReturned = new
            {
                Caption = "Top 3 least sold products per day",
                Products = top3LeastSoldProducts
            };

            return productsReturned;
        }

        /// <summary>
        /// Get the top 3 least sold products per month
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="month">Month of the sales</param>
        /// <returns>A list of the top 3 least sold products for a specific time period</returns>
        public object GetTop3LeastSoldProductsPerMonth(int year, int month)
        {
            List<MonthProductStatistics> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter monthParam = new("@month_param", SqlDbType.Int) { Value = month };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3LeastSoldProductsPerMonth] @year_param, @month_param";
            var results = _context.MonthProductStatistics.FromSqlRaw(sqlCommand, yearParam, monthParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new MonthProductStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Company = result.Company,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Order the products by MoneyEarnedFromSales in ascending order to get the least sold products
            var sortedProducts = products.OrderBy(p => p.MoneyEarnedFromSales).ToList();

            // Take the top 3 least sold products
            var top3LeastSoldProducts = sortedProducts.Take(3);

            // Create a new object that includes the caption and the products
            var productsReturned = new
            {
                Caption = "Top 3 least sold products per month",
                Products = top3LeastSoldProducts
            };

            return productsReturned;
        }

        /// <summary>
        /// Get the top 3 least sold products per quartile
        /// </summary>
        /// <param name="year">Year of the sales</param>
        /// <param name="quartile">Quartile of the sales</param>
        /// <returns>A list of the top 3 least sold products for a specific time period</returns>
        public object GetTop3LeastSoldProductsPerQuartile(int year, int quartile)
        {
            List<QuartileProductStatistics> products = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile_param", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[GetTop3LeastSoldProductsPerQuartile] @year_param, @quartile_param";
            var results = _context.QuartileProductStatistics.FromSqlRaw(sqlCommand, yearParam, quartileParam).ToList();

            // Map the stored procedure results to the Product objects
            foreach (var result in results)
            {
                products.Add(new QuartileProductStatistics
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Order the products by MoneyEarnedFromSales in ascending order to get the least sold products
            var sortedProducts = products.OrderBy(p => p.MoneyEarnedFromSales).ToList();

            // Take the top 3 least sold products
            var top3LeastSoldProducts = sortedProducts.Take(3);

            // Create a new object that includes the caption and the products
            var productsReturned = new
            {
                Caption = "Top 3 least sold products per quartile",
                Products = top3LeastSoldProducts
            };

            return productsReturned;
        }

        /// <summary>
        /// Function to get the purchases from suppliers per quartile
        /// </summary>
        /// <returns>A list of the purchases from suppliers per quartile</returns>
        public object GetPurchasesFromSuppliersPerQuartile()
        {
            List<QuartilePurchasesFromSuppliers> purchasesFromSuppliers = new();

            // Execute the stored procedure and map the results to Supplier
            string sqlCommand = "EXEC [dbo].[PurchasesFromSuppliersPerQuartile]";
            var results = _context.QuartilePurchasesFromSuppliers.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the Supplier objects
            foreach (var result in results)
            {
                purchasesFromSuppliers.Add(new QuartilePurchasesFromSuppliers
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    SupplierName = result.SupplierName,
                    SpentMoney = result.SpentMoney,
                    AmountOfProductsBought = result.AmountOfProductsBought
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
        /// <returns>A list of the most sold products per quartile</returns>
        public object GetMostSoldProductsPerQuartile()
        {
            // Create a list to store the most sold products
            List<QuartileProductStatistics> mostSoldProducts = new();

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[MostSoldProductsByQuartile]";
            var results = _context.QuartileProductStatistics.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the TopProduct objects
            foreach (var result in results)
            {
                mostSoldProducts.Add(new QuartileProductStatistics
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Create a new object that includes the caption and the suppliers
            var mostSoldProductsReturned = new
            {
                Caption = "List of the most sold products per quartile",
                Products = mostSoldProducts
            };

            return mostSoldProductsReturned;
        }

        /// <summary>
        /// Function to get the most sold products per month
        /// </summary>
        /// <returns>A list of the most sold products per month</returns>
        public object GetMostSoldProductsPerMonth()
        {
            // Create a list to store the most sold products
            List<MonthProductStatistics> mostSoldProducts = new();

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[MostSoldProductsByMonth]";
            var results = _context.MonthProductStatistics.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the TopProduct objects
            foreach (var result in results)
            {
                mostSoldProducts.Add(new MonthProductStatistics
                {
                    Year = result.Year,
                    Month = result.Month,
                    Company = result.Company,
                    Family = result.Family,
                    Description = result.Description,
                    MoneyEarnedFromSales = result.MoneyEarnedFromSales
                });
            }

            // Create a new object that includes the caption and the suppliers
            var mostSoldProductsReturned = new
            {
                Caption = "List of the most sold products per month",
                Products = mostSoldProducts
            };

            return mostSoldProductsReturned;
        }

        /// <summary>
        /// Function to get the average daily sales per quartile
        /// </summary>
        /// <returns>The list of the average daily sales per quartile</returns>
        public object GetAverageDailySalesPerQuartile()
        {
            // Create a list to store the average daily sales
            List<QuartileAverageDailySales> averageDailySales = new();

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[AverageDailySalesByQuartile]";
            var results = _context.AverageDailySales.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the TopProduct objects
            foreach (var result in results)
            {
                averageDailySales.Add(new QuartileAverageDailySales
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    Money = result.Money
                });
            }

            // Create a new object that includes the caption and the sales
            var averageDailySalesReturned = new
            {
                Caption = "List of the most sold products per quartile",
                Sales = averageDailySales
            };

            return averageDailySalesReturned;
        }

        /// <summary>
        /// Function to get the montly sales mode by quartile
        /// </summary>
        /// <returns>The list of the montly sales mode by quartile</returns>
        public object GetMonthSalesModeByQuartile()
        {
            // Create a list to store the average daily sales
            List<QuartileMonthSalesMode> monthlySalesMode = new();

            // Execute the stored procedure and map the results to Product
            string sqlCommand = "EXEC [dbo].[MonthSalesModeByQuartile]";
            var results = _context.MonthSalesModes.FromSqlRaw(sqlCommand).ToList();

            // Map the stored procedure results to the TopProduct objects
            foreach (var result in results)
            {
                monthlySalesMode.Add(new QuartileMonthSalesMode
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    Money = result.Money
                });
            }

            // Create a new object that includes the caption and the products
            var monthlySalesModeReturned = new
            {
                Caption = "List of the montly sales mode by quartile",
                MonthlySalesMode = monthlySalesMode
            };

            return monthlySalesModeReturned;
        }

        /// <summary>
        /// Function to get the cities by quartile sales
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quartile"></param>
        /// <returns>The list of the cities by quartile sales</returns></returns>
        public object GetCitiesByQuartileSales(int year, int quartile)
        {
            List<QuartileCity> cities = new();

            // Create parameters for the stored procedure
            SqlParameter yearParam = new("@year_param", SqlDbType.Int) { Value = year };
            SqlParameter quartileParam = new("@quartile_param", SqlDbType.Int) { Value = quartile };

            // Execute the stored procedure and map the results to City
            string sqlCommand = "EXEC [dbo].[GetCitiesByQuartileSales] @year_param, @quartile_param";
            var results = _context.CitySalesStatistics.FromSqlRaw(sqlCommand, yearParam, quartileParam).ToList();

            // Map the stored procedure results to the City objects
            foreach (var result in results)
            {
                cities.Add(new QuartileCity
                {
                    Year = result.Year,
                    Quartile = result.Quartile,
                    Company = result.Company,
                    City = result.City,
                    TotalSales = result.TotalSales
                });
            }

            // Create a new object that includes the caption and the cities
            var citiesReturned = new
            {
                Caption = "Cities by quartile sales",
                Cities = cities
            };

            return citiesReturned;

        }
    }
}