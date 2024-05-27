namespace Sio2324_Grupo_03.Models
{
    /// <summary>
    /// Quartile product statistics class
    /// </summary>
    public class QuartileProductStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The quartile of the year in which the product was sold --> may be null depending on the procedure that was executed
        public string? Quartile { get; set; } = null;

        // The family of the product --> may be null depending on the procedure that was executed. Example: APPLE, ASUS, SAMSUNG, etc.
        public string? Family { get; set; } = null;

        // The name of the product --> may be null depending on the procedure that was executed. Example: iPhone 13 Pro, ZenBook Pro, Galaxy S21, etc.
        public string? Description { get; set; } = null;

        // The amount of money earned by the company from the sales of the product in a specific time period
        public decimal MoneyEarnedFromSales { get; set; } = 0;
    }

    /// <summary>
    /// Monthly product statistics class
    /// </summary>
    public class MonthProductStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The month in which the product was sold
        public int Month { get; set; } = 0;

        // The family of the product --> may be null depending on the procedure that was executed. Example: APPLE, ASUS, SAMSUNG, etc.
        public string? Family { get; set; } = null;

        // The name of the product --> may be null depending on the procedure that was executed. Example: iPhone 13 Pro, ZenBook Pro, Galaxy S21, etc.
        public string? Description { get; set; } = null;

        // The amount of money earned by the company from the sales of the product in a specific time period
        public decimal MoneyEarnedFromSales { get; set; } = 0;
    }

    /// <summary>
    /// Daily product statistics class
    /// </summary>
    public class DailyProductStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The month in which the product was sold
        public int Month { get; set; } = 0;

        // The day in which the product was sold
        public int Day { get; set; } = 0;

        // The family of the product --> may be null depending on the procedure that was executed. Example: APPLE, ASUS, SAMSUNG, etc.
        public string? Family { get; set; } = null;

        // The name of the product --> may be null depending on the procedure that was executed. Example: iPhone 13 Pro, ZenBook Pro, Galaxy S21, etc.
        public string? Description { get; set; } = null;

        // The amount of money earned by the company from the sales of the product in a specific time period
        public decimal MoneyEarnedFromSales { get; set; } = 0;
    }
}