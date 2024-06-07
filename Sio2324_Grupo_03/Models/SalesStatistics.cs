namespace Sio2324_Grupo_03.Models
{
    /// <summary>
    /// Quartile sales statistics class
    /// </summary>
    public class QuartileSalesStatistics
    {
        // The year in which the sale was made
        public int? Year { get; set; } = null;

        // The quartile in which the sale was made
        public string? Quartile { get; set; } = null;

        // Id of the company that bought the product
        public int? Company { get; set; } = null;

        // The net amount of money earned by the company from the sales of the products in a specific time period
        public decimal? NetAmountEarned { get; set; } = 0;

        // The gross amount of money earned by the company from the sales of the products in a specific time period
        public decimal? GrossAmountEarned { get; set; } = 0;

        // The quantity of products sold in a specific time period
        public int? QuantitySold { get; set; } = 0;
    }

    /// <summary>
    /// Monthly sales statistics class
    /// </summary>
    public class MonthSalesStatistics
    {
        // The year in which the sale was made
        public int? Year { get; set; } = 0;

        // The month in which the sale was made
        public int? Month { get; set; } = 0;

        // Id of the company that bought the product
        public int? Company { get; set; } = 0;

        // The net amount of money earned by the company from the sales of the products in a specific time period
        public decimal? NetAmountEarned { get; set; } = 0;

        // The gross amount of money earned by the company from the sales of the products in a specific time period
        public decimal? GrossAmountEarned { get; set; } = 0;

        // The quantity of products sold in a specific time period
        public int? QuantitySold { get; set; } = 0;
    }

    /// <summary>
    /// Daily sales statistics class
    /// </summary>
    public class DailySalesStatistics
    {
        // The year in which the sale was made
        public int? Year { get; set; } = 0;

        // The month in which the sale was made
        public int? Month { get; set; } = 0;

        // The day in which the sale was made
        public int? Day { get; set; } = 0;

        // Id of the company that bought the product
        public int? Company { get; set; } = 0;

        // The net amount of money earned by the company from the sales of the products in a specific time period
        public decimal? NetAmountEarned { get; set; } = 0;

        // The gross amount of money earned by the company from the sales of the products in a specific time period
        public decimal? GrossAmountEarned { get; set; } = 0;

        // The quantity of products sold in a specific time period
        public int? QuantitySold { get; set; } = 0;
    }

    /// <summary>
    /// Quartile average daily sales class
    /// </summary>
    public class QuartileAverageDailySales
    {
        // The year in which the sales was made
        public int? Year { get; set; }

        // The quartile of the year in which the sales was made
        public string? Quartile { get; set; }

        // Id of the company that bought the product
        public int? Company { get; set; } = 0;

        // The amount of money earned by the company from the sales in a specific time period
        public decimal Money { get; set; }
    }

    /// <summary>
    /// Monthly sales mode class
    /// </summary>
    public class QuartileMonthSalesMode
    {
        // The year in which the sales was made
        public int? Year { get; set; }

        // The quartile of the year in which the sales was made
        public string? Quartile { get; set; }

        // Id of the company that bought the product
        public int? Company { get; set; } = 0;

        // The amount of money earned by the company from the sales in a specific time period
        public decimal Money { get; set; }
    }

    /// <summary>
    /// Quartile city sales statistics class
    /// </summary>
    public class QuartileCity
    {
        // The year in which the sales were made
        public int? Year { get; set; } = 0;

        // The quartile in which the sales were made
        public string? Quartile { get; set; } = null;

        public int? Company { get; set; } = 0; // [Company]

        // The city in which the sales were made
        public string? City { get; set; } = null;

        // The total sales made in the city in a specific time period
        public decimal? TotalSales { get; set; } = 0;
    }
}