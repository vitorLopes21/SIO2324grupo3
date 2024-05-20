namespace Sio2324_Grupo_03.Models
{
    public class Product
    {
        // The quartile of the year in which the product was sold --> may be null depending on the procedure that was executed
        public string? Quartile { get; set; }

        // The family of the product --> may be null depending on the procedure that was executed. Example: APPLE, ASUS, SAMSUNG, etc.
        public string? Family { get; set; }

        // The name of the product --> may be null depending on the procedure that was executed. Example: iPhone 13 Pro, ZenBook Pro, Galaxy S21, etc.
        public string? Description { get; set; }

        // The amount of money earned by the company from the sales of the product in a specific time period
        public decimal MoneyEarnedFromSales { get; set; }
    }
}