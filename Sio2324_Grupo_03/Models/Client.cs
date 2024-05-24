namespace Sio2324_Grupo_03.Models
{
    public class Client
    {
        // The year in which the product was sold
        public int Year { get; set; } = 0;

        // The month in which the product was sold
        public int Month { get; set; } = 0;

        // The day in which the product was sold
        public int Day { get; set; } = 0;

        // The quartile of the year in which the product was sold
        public string? Quartile { get; set; } = null;

        // The name of the client
        public string? ClientName { get; set; } = null;

        // The amount of money spent by the client in a specific time period
        public double SpentMoney { get; set; } = 0;
    }
}