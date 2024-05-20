namespace Sio2324_Grupo_03.Controllers
{
    [Route("api")]
    [ApiController]
    public class CInformationExpert : ControllerBase
    {
        private readonly IInformationExpert _informationService;
        private readonly ILogger<CInformationExpert> _logger;

        public CInformationExpert(ILogger<CInformationExpert> logger, IInformationExpert informationService)
        {
            _informationService = informationService;
            _logger = logger;
        }

        [HttpGet("salesstatsforquartile")]
        public IActionResult GetSalesStatsForQuartile(int year, int quartile)
        {
            try
            {
                // Calculate sales statistics for the specified quartile
                var salesStats = _informationService.CalculateSalesStatsForQuartile(year, quartile);

                // Return HTTP 200 OK response with the calculated sales statistics data
                /*
                return new JsonResult(new SalesStatistics
                {
                    GrossAmountQuartile = 10,
                    NetAmountEarnedQuartile = 11,
                    QuantitySalesQuartile = 5
                });*/
                return Ok(salesStats);
            }
            catch (Exception ex)
            {
                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating sales statistics for quartile.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quartile"></param>
        /// <returns></returns>
        [HttpGet("purchases-from-suppliers-per-quartile")]
        public IActionResult GetPurchasesFromSuppliersPerQuartile()
        {
            try
            {
                // Calculate purchases from suppliers for the specified quartile
                var purchasesFromSuppliers = _informationService.GetPurchasesFromSuppliersPerQuartile();

                // Create the returning json object
                var supplierObjects = purchasesFromSuppliers.Select(supplier => new
                {
                    supplier.Quartile,
                    supplier.Supplier,
                    supplier.SpentMoney
                }).ToList();

                // Create a new object that includes the caption and the suppliers
                var jsonObjectReturned = new
                {
                    Caption = "List of purchases from suppliers per quartile",
                    Suppliers = supplierObjects
                };

                // Return HTTP 200 OK response with the calculated purchases from suppliers data
                return Ok(jsonObjectReturned);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating purchases from suppliers for quartile.");
                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating purchases from suppliers for quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the most sold products per quartile
        /// </summary>
        /// <returns>The most sold products per quartile</returns>
        [HttpGet("most-sold-products-per-quartile")]
        public IActionResult GetMostSoldProductsPerQuartile()
        {
            try
            {
                // Retrieve the ranking of the most sold products per quartile
                var mostSoldProducts = _informationService.GetMostSoldProductsPerQuartile();

                // Create the returning json object
                var productObjects = mostSoldProducts.Select(product => new
                {
                    product.Quartile,
                    product.Family,
                    product.Description,
                    product.MoneyEarnedFromSales
                }).ToList();

                // Create a new object that includes the caption and the products
                var jsonObjectReturned = new
                {
                    Caption = "List of the most sold products per quartile",
                    Products = productObjects
                };

                // Return HTTP 200 OK response with the most sold products data
                return Ok(jsonObjectReturned);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the most sold products per quartile.");
                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the most sold products per quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the average daily sales per quartile
        /// </summary>
        /// <returns>The average daily sales per quartile</returns>
        [HttpGet("avg-daily-sales-per-quartile")]
        public IActionResult GetAVGDailySalesPerQuartile()
        {
            try
            {
                // Retrieve the ranking of the most sold products per quartile
                var AVGDailySales = _informationService.GetAVGDailySalesPerQuartile();

                // Create the returning json object
                var productObjects = AVGDailySales.Select(sales => new
                {
                    sales.Year,
                    sales.Quartile,
                    sales.Money
                }).ToList();

                // Create a new object that includes the caption and the products
                var jsonObjectReturned = new
                {
                    Caption = "List of the avg daily sales per quartile",
                    Products = productObjects
                };

                // Return HTTP 200 OK response with the most sold products data
                return Ok(jsonObjectReturned);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the avg daily sales per quartile.");
                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the avg daily sales per quartile.");
            }
        }
    }
}