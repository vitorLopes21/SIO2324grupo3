namespace Sio2324_Grupo_03.Interfaces
{
    public interface IInformationExpert
    {
        // Method to calculate sales statistics for a time period in a year
        object CalculateSalesStatsForQuartile(int year, int quartile);

        object CalculateSalesStatsForMonth(int year, int month);

        object CalculateSalesStatsForDay(int year, int month, int day);

        // Method to get the top 3 sold products for a specific time period
        object GetTop3SoldProductsPerQuartile(int year, int quartile);

        object GetTop3SoldProductsPerMonth(int year, int month);

        object GetTop3SoldProductsPerDay(int year, int month, int day);

        // Method to retrieve the top 3 clients by sales value for a specific time period
        object GetTop3ClientsByValuePerQuartile(int year, int quartile);

        object GetTop3ClientsByValuePerMonth(int year, int month);

        object GetTop3ClientsByValuePerDay(int year, int month, int day);

        // Method to retrieve the ranking of the most sold products per quartile
        object GetMostSoldProductsPerQuartile();

        // Method to retrieve the purchases made to suppliers per quartile
        object GetPurchasesFromSuppliersPerQuartile();
    }
}