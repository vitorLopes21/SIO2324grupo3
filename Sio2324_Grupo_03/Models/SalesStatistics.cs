namespace Sio2324_Grupo_03.Models
{
    public class SalesStatistics
    {
        // The year in which the sale was made
        public int Year { get; set; } = 0;

        // The month in which the sale was made
        public int Month { get; set; } = 0;

        // The day in which the sale was made
        public int Day { get; set; } = 0;

        // The net amount of money earned by the company from the sales of the products in a specific time period
        public double NetAmountEarned { get; set; } = 0;

        // The gross amount of money earned by the company from the sales of the products in a specific time period
        public double GrossAmountEarned { get; set; } = 0;

        // The quantity of products sold in a specific time period
        public int QuantitySales { get; set; } = 0;
    }
}