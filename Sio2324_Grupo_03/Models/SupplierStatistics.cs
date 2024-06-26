﻿namespace Sio2324_Grupo_03.Models
{
    /// <summary>
    /// Quartile supplier statistics class
    /// </summary>
    public class QuartileSupplierStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The quartile of the year
        public string? Quartile { get; set; } = null;

        // Id of the company that bought the product
        public int? Company { get; set; } = null;

        // The name of the supplier
        public string? SupplierName { get; set; } = null;

        // The city where the supplier is located
        public string? City { get; set; } = null;

        // The market where the supplier operates
        public string? Market { get; set; } = null;

        // The amount of money spent by the company in a specific quartile
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the company in a specific time period
        public int BoughtProducts { get; set; } = 0;
    }

    /// <summary>
    /// Monthly supplier statistics class
    /// </summary>
    public class MonthSupplierStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The month in which the product was sold
        public int Month { get; set; } = 0;

        // Id of the company that bought the product
        public int? Company { get; set; } = null;

        // The name of the supplier
        public string? SupplierName { get; set; } = null;

        // The city where the supplier is located
        public string? City { get; set; } = null;

        // The market where the supplier operates
        public string? Market { get; set; } = null;

        // The amount of money spent by the company in a specific quartile
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the company in a specific time period
        public int BoughtProducts { get; set; } = 0;
    }

    /// <summary>
    /// Daily supplier statistics class
    /// </summary>
    public class DailySupplierStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The month in which the product was sold
        public int Month { get; set; } = 0;

        // The day in which the product was sold
        public int Day { get; set; } = 0;

        // Id of the company that bought the product
        public int? Company { get; set; } = null;

        // The name of the supplier
        public string? SupplierName { get; set; } = null;

        // The city where the supplier is located
        public string? City { get; set; } = null;

        // The market where the supplier operates
        public string? Market { get; set; } = null;

        // The amount of money spent by the company in a specific quartile
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the company in a specific time period
        public int BoughtProducts { get; set; } = 0;
    }

    public class QuartilePurchasesFromSuppliers 
    { 
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The quartile of the year
        public string? Quartile { get; set; } = null;

        // Id of the company that bought the product
        public int? Company { get; set; } = null;

        // Name of the supplier
        public string? SupplierName { get; set; } = null;

        // Money spent in the quartile
        public decimal SpentMoney { get; set; } = 0;

        // Amount of products bought in the quartile
        public int AmountOfProductsBought { get; set; } = 0;
    }
}