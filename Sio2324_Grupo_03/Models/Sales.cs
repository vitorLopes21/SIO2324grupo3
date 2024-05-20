namespace Sio2324_Grupo_03.Models
{
    public class Sales
    {
        // The year in which the sales --> may be null depending on the procedure that was executed
        public string? Year { get; set; }

        // The quartile of the year in which the sales --> may be null depending on the procedure that was executed
        public string? Quartile { get; set; }


        // The amount of money earned by the company from the sales in a specific time period
        public decimal Money { get; set; }
    }
}