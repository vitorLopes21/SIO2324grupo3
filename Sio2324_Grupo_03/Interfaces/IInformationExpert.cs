namespace Sio2324_Grupo_03.Interfaces
{
    public interface IInformationExpert
    {
        // Method to calculate sales statistics for a time period in a year
        SalesStatistics CalculateSalesStatsForQuartile(int year, int quartile);

        SalesStatistics CalculateSalesStatsForMonth(int year, int month);

        SalesStatistics CalculateSalesStatsForDay(int year, int month, int day);

        // Method to get the top 3 sold products for a specific time period
        List<TopProduct> GetTop3SoldProductsPerQuartile(int year, int quartile);

        List<TopProduct> GetTop3SoldProductsPerMonth(int year, int month);

        List<TopProduct> GetTop3SoldProductsPerDay(int year, int month, int day);

        // Method to retrieve the top 3 clients by sales value for a specific time period
        List<TopClient> GetTop3ClientsByValuePerQuartile(int year, int quartile);

        List<TopClient> GetTop3ClientsByValuePerMonth(int year, int month);

        List<TopClient> GetTop3ClientsByValuePerDay(int year, int month, int day);

    }

}
