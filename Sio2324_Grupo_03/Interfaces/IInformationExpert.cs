namespace Sio2324_Grupo_03.Interfaces
{
    public interface IInformationExpert
    {
        // Method to calculate sales statistics for a time period in a year
        object CalculateSalesStatsForQuartile(int year, int quartile);

        object CalculateSalesStatsForMonth(int year, int month);

        object CalculateSalesStatsForDay(int year, int month, int day);

        // Method to get the top 3 sold products for a specific time period
        object GetTop3MostSoldProductsPerQuartile(int year, int quartile);

        object GetTop3MostSoldProductsPerMonth(int year, int month);

        object GetTop3MostSoldProductsPerDay(int year, int month, int day);

        //Method to get the top 3 least sold products for a specific time period
        object GetTop3LeastSoldProductsPerQuartile(int year, int quartile);

        object GetTop3LeastSoldProductsPerMonth(int year, int month);

        object GetTop3LeastSoldProductsPerDay(int year, int month, int day);

        // Method to retrieve the top 3 clients by sales value for a specific time period
        object GetTop3ClientsByValuePerQuartile(int year, int quartile);

        object GetTop3ClientsByValuePerMonth(int year, int month);

        object GetTop3ClientsByValuePerDay(int year, int month, int day);

        // Method to retrieve the top 3 suppliers by sales value for a specific time period
        object GetTop3SuppliersByValuePerQuartile(int year, int quartile);

        object GetTop3SuppliersByValuePerMonth(int year, int month);

        object GetTop3SuppliersByValuePerDay(int year, int month, int day);

        // Method to retrieve the ranking of the most sold products per quartile
        object GetMostSoldProductsPerQuartile();

        // Method to retrieve the purchases made to suppliers per quartile
        object GetPurchasesFromSuppliersPerQuartile();

        // Method to get average daily sales over the quartiles of a year
        object GetAverageDailySalesPerQuartile();

        // Method to retrieve the ranking of the montly sales mode by quartile
        object GetMonthSalesModeByQuartile();
    }
}