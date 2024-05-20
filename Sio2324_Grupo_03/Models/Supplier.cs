namespace Sio2324_Grupo_03.Models
{
    public class Supplier
    {
        // The quartile of the year
        public string? Quartile { get; set; }

        // The name of the supplier
        public string? Supplier { get; set; }

        // The amount of money spent by the company in a specific quartile
        public double SpentMoney { get; set; }

        // The amount of products bought by the company in a specific time period
        public int AmountOfProductsBought { get; set; }
    }
}