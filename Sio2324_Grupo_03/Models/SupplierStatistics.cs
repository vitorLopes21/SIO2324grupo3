namespace Sio2324_Grupo_03.Models
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

        // The name of the supplier
        public string? SupplierName { get; set; } = null;

        // The amount of money spent by the company in a specific quartile
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the company in a specific time period
        public int AmountOfProductsBought { get; set; } = 0;
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

        // The name of the supplier
        public string? SupplierName { get; set; } = null;

        // The amount of money spent by the company in a specific month
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the company in a specific time period
        public int AmountOfProductsBought { get; set; } = 0;
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

        // The name of the supplier
        public string? SupplierName { get; set; } = null;

        // The amount of money spent by the company in a specific quartile
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the company in a specific time period
        public int AmountOfProductsBought { get; set; } = 0;
    }
}