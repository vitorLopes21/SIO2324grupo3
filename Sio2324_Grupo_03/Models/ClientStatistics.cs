namespace Sio2324_Grupo_03.Models
{
    /// <summary>
    /// Quartile client statistics class
    /// </summary>
    public class QuartileClientStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The quartile of the year in which the product was sold
        public string? Quartile { get; set; } = null;

        // The name of the client
        public string? ClientName { get; set; } = null;

        // The amount of money spent by the client in a specific time period
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the client in a specific time period
        public int BoughtProducts { get; set; } = 0;
    }

    /// <summary>
    /// Monthly client statistics class
    /// </summary>
    public class MonthClientStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The month in which the product was sold
        public int Month { get; set; } = 0;

        // The name of the client
        public string? ClientName { get; set; } = null;

        // The amount of money spent by the client in a specific time period
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the client in a specific time period
        public int BoughtProducts { get; set; } = 0;
    }

    /// <summary>
    /// Daily client statistics class
    /// </summary>
    public class DailyClientStatistics
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The month in which the product was sold
        public int Month { get; set; } = 0;

        // The day in which the product was sold
        public int Day { get; set; } = 0;

        // The name of the client
        public string? ClientName { get; set; } = null;

        // The amount of money spent by the client in a specific time period
        public decimal SpentMoney { get; set; } = 0;

        // The amount of products bought by the client in a specific time period
        public int BoughtProducts { get; set; } = 0;
    }
}