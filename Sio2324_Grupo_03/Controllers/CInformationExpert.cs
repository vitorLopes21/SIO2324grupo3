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

        /// <summary>
        /// Function to get the sales statistics for a specific period of time
        /// </summary>
        /// <param name="year">Year of the sales statistics</param>
        /// <param name="quartile">Quartile of the year, from 1 to 4</param>
        /// <returns>The sales statistics for the specified quartile</returns>
        [HttpGet("sales-stats-for-quartile")]
        public IActionResult GetSalesStatsForQuartile([FromQuery] int year = 2023, [FromQuery] int quartile = 1)
        {
            try
            {
                // Calculate sales statistics for the specified quartile
                var salesStats = _informationService.CalculateSalesStatsForQuartile(year, quartile);

                // Return HTTP 200 OK response with the calculated sales statistics data
                return Ok(salesStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating sales statistics for quartile.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating sales statistics for quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the sales statistics for a specific month
        /// </summary>
        /// <param name="year">Year of the sales statistics</param>
        /// <param name="month">Month of the sales statistics</param>
        /// <returns>The sales statistics for the specified month</returns>
        [HttpGet("sales-stats-for-month")]
        public IActionResult GetSalesStatsForMonth([FromQuery] int year = 2023, [FromQuery] int month = 01)
        {
            try
            {
                // Calculate sales statistics for the specified month
                var salesStats = _informationService.CalculateSalesStatsForMonth(year, month);

                // Return HTTP 200 OK response with the calculated sales statistics data
                return Ok(salesStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating sales statistics for month.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating sales statistics for month.");
            }
        }


        /// <summary>
        /// HTTP GET method to retrieve the sales statistics for a specific day
        /// </summary>
        /// <param name="year">Year of the sales statistics</param>
        /// <param name="month">Month of the sales statistics</param>
        /// <param name="day">Day of the sales statistics</param>
        /// <returns>The sales statistics for the specified day</returns>
        [HttpGet("sales-stats-for-day")]
        public IActionResult GetSalesStatsForDay([FromQuery] int year = 2023, [FromQuery] int month = 01, [FromQuery] int day = 02)
        {
            try
            {
                // Calculate sales statistics for the specified day
                var salesStats = _informationService.CalculateSalesStatsForDay(year, month, day);

                // Return HTTP 200 OK response with the calculated sales statistics data
                return Ok(salesStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating sales statistics for day.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating sales statistics for day.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the product statistics for a specific quartile
        /// </summary>
        /// <returns> The product movements statistics for the specified quartile</returns>
        [HttpGet("product-movements-stats-for-quartile")]
        public IActionResult GetProductMovementsStatsForQuartile()
        {
            try
            {
                // Calculate product statistics for the specified quartile
                var productStats = _informationService.CalculateProductStatsForQuartile();

                // Return HTTP 200 OK response with the calculated product statistics data
                return Ok(productStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating product statistics for quartile.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating product statistics for quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the product statistics for a specific month
        /// </summary>
        /// <returns> The product movements statistics for the specified month </returns>
        [HttpGet("product-movements-stats-for-month")]
        public IActionResult GetProductMovementsStatsForMonth()
        {
            try
            {
                // Calculate product statistics for the specified month
                var productStats = _informationService.CalculateProductStatsForMonth();

                // Return HTTP 200 OK response with the calculated product statistics data
                return Ok(productStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating product statistics for month.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating product statistics for month.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 sold products for a specific quartile
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="quartile">Quartile of the year, from 1 to 4</param>
        /// <returns>A list of the top 3 sold products for the specified quartile</returns>
        [HttpGet("top-3-most-sold-products-per-quartile")]
        public IActionResult GetTop3MostSoldProductsPerQuartile([FromQuery] int year = 2023, [FromQuery] int quartile = 1)
        {
            try
            {
                // Retrieve the top 3 sold products for the specified quartile
                var top3SoldProducts = _informationService.GetTop3MostSoldProductsPerQuartile(year, quartile);

                // Return HTTP 200 OK response with the top 3 sold products data
                return Ok(top3SoldProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 sold products per quartile.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 sold products per quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 sold products for a specific month
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="month">Month of the time period being analyzed</param>
        /// <returns>A list of the top 3 sold products for the specified month</returns>
        [HttpGet("top-3-most-sold-products-per-month")]
        public IActionResult GetTop3MostSoldProductsPerMonth([FromQuery] int year = 2023, [FromQuery] int month = 01)
        {
            try
            {
                // Retrieve the top 3 sold products for the specified month
                var top3SoldProducts = _informationService.GetTop3MostSoldProductsPerMonth(year, month);

                // Return HTTP 200 OK response with the top 3 sold products data
                return Ok(top3SoldProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 sold products per month.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 sold products per month.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 sold products for a specific day
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="month">Month of the time period being analyzed</param>
        /// <param name="day">Day of the time period being analyzed</param>
        /// <returns>A list of the top 3 sold products for the specified day</returns>
        [HttpGet("top-3-most-sold-products-per-day")]
        public IActionResult GetTop3MostSoldProductsPerDay([FromQuery] int year = 2023, [FromQuery] int month = 01, [FromQuery] int day = 02)
        {
            try
            {
                // Retrieve the top 3 sold products for the specified day
                var top3SoldProducts = _informationService.GetTop3MostSoldProductsPerDay(year, month, day);

                // Return HTTP 200 OK response with the top 3 sold products data
                return Ok(top3SoldProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 sold products per day.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 sold products per day.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 least sold products for a specific quartile
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="quartile">Quartile of the year, from 1 to 4</param>
        /// <returns>A list of the top 3 least sold products for the specified quartile</returns>
        [HttpGet("top-3-least-sold-products-per-quartile")]
        public IActionResult GetTop3LeastSoldProductsPerQuartile([FromQuery] int year = 2023, [FromQuery] int quartile = 1)
        {
            try
            {
                // Retrieve the top 3 least sold products for the specified quartile
                var top3LeastSoldProducts = _informationService.GetTop3LeastSoldProductsPerQuartile(year, quartile);

                // Return HTTP 200 OK response with the top 3 least sold products data
                return Ok(top3LeastSoldProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 least sold products per quartile.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 least sold products per quartile.");
            }
        }

        /// HTTP GET method to retrieve the top 3 least sold products for a specific month
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="month">Month of the time period being analyzed</param>
        /// <returns>A list of the top 3 least sold products for the specified month</returns>
        [HttpGet("top-3-least-sold-products-per-month")]
        public IActionResult GetTop3LeastSoldProductsPerMonth([FromQuery] int year = 2023, [FromQuery] int month = 01)
        {
            try
            {
                // Retrieve the top 3 least sold products for the specified month
                var top3LeastSoldProducts = _informationService.GetTop3LeastSoldProductsPerMonth(year, month);

                // Return HTTP 200 OK response with the top 3 least sold products data
                return Ok(top3LeastSoldProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 least sold products per month.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 least sold products per month.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 least sold products for a specific day
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="month">Month of the time period being analyzed</param>
        /// <param name="day">Day of the time period being analyzed</param>
        /// <returns>A list of the top 3 least sold products for the specified day</returns>
        [HttpGet("top-3-least-sold-products-per-day")]
        public IActionResult GetTop3LeastSoldProductsPerDay([FromQuery] int year = 2023, [FromQuery] int month = 01, [FromQuery] int day = 02)
        {
            try
            {
                // Retrieve the top 3 least sold products for the specified day
                var top3LeastSoldProducts = _informationService.GetTop3LeastSoldProductsPerDay(year, month, day);

                // Return HTTP 200 OK response with the top 3 least sold products data
                return Ok(top3LeastSoldProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 least sold products per day.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 least sold products per day.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 suppliers by sales value for a specific quartile
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="quartile">Quartile of the year, from 1 to 4</param>
        /// <returns>A list of the top 3 suppliers by sales value for the specified quartile</returns>
        [HttpGet("top-3-suppliers-by-value-per-quartile")]
        public IActionResult GetTop3SuppliersByValuePerQuartile([FromQuery] int year = 2023, [FromQuery] int quartile = 1)
        {
            try
            {
                // Retrieve the top 3 suppliers by sales value for the specified quartile
                var top3Suppliers = _informationService.GetTop3SuppliersByValuePerQuartile(year, quartile);

                // Return HTTP 200 OK response with the top 3 suppliers data
                return Ok(top3Suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 suppliers by sales value per quartile.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 suppliers by sales value per quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 suppliers by sales value for a specific month
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="month">Month of the time period being analyzed</param>
        /// <returns>A list of the top 3 suppliers by sales value for the specified month</returns>
        [HttpGet("top-3-suppliers-by-value-per-month")]
        public IActionResult GetTop3SuppliersByValuePerMonth([FromQuery] int year = 2023, [FromQuery] int month = 01)
        {
            try
            {
                // Retrieve the top 3 suppliers by sales value for the specified month
                var top3Suppliers = _informationService.GetTop3SuppliersByValuePerMonth(year, month);

                // Return HTTP 200 OK response with the top 3 suppliers data
                return Ok(top3Suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 suppliers by sales value per month.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 suppliers by sales value per month.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 suppliers by sales value for a specific day
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="month">Month of the time period being analyzed</param>
        /// <param name="day">Day of the time period being analyzed</param>
        /// <returns>A list of the top 3 suppliers by sales value for the specified day</returns>
        [HttpGet("top-3-suppliers-by-value-per-day")]
        public IActionResult GetTop3SuppliersByValuePerDay([FromQuery] int year = 2023, [FromQuery] int month = 01, [FromQuery] int day = 02)
        {
            try
            {
                // Retrieve the top 3 suppliers by sales value for the specified day
                var top3Suppliers = _informationService.GetTop3SuppliersByValuePerDay(year, month, day);

                // Return HTTP 200 OK response with the top 3 suppliers data
                return Ok(top3Suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 suppliers by sales value per day.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 suppliers by sales value per day.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 clients by sales value for a specific quartile
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="quartile">Quartile of the year, from 1 to 4</param>
        /// <returns>A list of the top 3 clients by sales value for the specified quartile</returns>
        [HttpGet("top-3-clients-by-value-per-quartile")]
        public IActionResult GetTop3ClientsByValuePerQuartile([FromQuery] int year = 2023, [FromQuery] int quartile = 1)
        {
            try
            {
                // Retrieve the top 3 clients by sales value for the specified quartile
                var top3Clients = _informationService.GetTop3ClientsByValuePerQuartile(year, quartile);

                // Return HTTP 200 OK response with the top 3 clients data
                return Ok(top3Clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 clients by sales value per quartile.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 clients by sales value per quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 clients by sales value for a specific month
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="month">Month of the time period being analyzed</param>
        /// <returns>A list of the top 3 clients by sales value for the specified month</returns>
        [HttpGet("top-3-clients-by-value-per-month")]
        public IActionResult GetTop3ClientsByValuePerMonth([FromQuery] int year = 2023, [FromQuery] int month = 01)
        {
            try
            {
                // Retrieve the top 3 clients by sales value for the specified month
                var top3Clients = _informationService.GetTop3ClientsByValuePerMonth(year, month);

                // Return HTTP 200 OK response with the top 3 clients data
                return Ok(top3Clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 clients by sales value per month.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 clients by sales value per month.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the top 3 clients by sales value for a specific day
        /// </summary>
        /// <param name="year">Year of the time period being analyzed</param>
        /// <param name="month">Month of the time period being analyzed</param>
        /// <param name="day">Day of the time period being analyzed</param>
        /// <returns>A list of the top 3 clients by sales value for the specified day</returns>
        [HttpGet("top-3-clients-by-value-per-day")]
        public IActionResult GetTop3ClientsByValuePerDay([FromQuery] int year = 2023, [FromQuery] int month = 01, [FromQuery] int day = 02)
        {
            try
            {
                // Retrieve the top 3 clients by sales value for the specified day
                var top3Clients = _informationService.GetTop3ClientsByValuePerDay(year, month, day);

                // Return HTTP 200 OK response with the top 3 clients data
                return Ok(top3Clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the top 3 clients by sales value per day.");

                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the top 3 clients by sales value per day.");
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
                var jsonObjectReturned = _informationService.GetMostSoldProductsPerQuartile();

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
        /// HTTP GET method to retrieve the most sold products per month
        /// </summary>
        /// <returns>The most sold products per month</returns>
        [HttpGet("most-sold-products-per-month")]
        public IActionResult GetMostSoldProductsPerMonth()
        {
            try
            {
                // Retrieve the ranking of the most sold products per month
                var jsonObjectReturned = _informationService.GetMostSoldProductsPerMonth();

                // Return HTTP 200 OK response with the most sold products data
                return Ok(jsonObjectReturned);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the most sold products per month.");
                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the most sold products per month.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet("purchases-from-suppliers-per-quartile")]
        public IActionResult GetPurchasesFromSuppliersPerQuartile()
        {
            try
            {
                // Calculate purchases from suppliers for the specified quartile
                var purchasesFromSuppliers = _informationService.GetPurchasesFromSuppliersPerQuartile();

                // Return HTTP 200 OK response with the calculated purchases from suppliers data
                return Ok(purchasesFromSuppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating purchases from suppliers for quartile.");
                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating purchases from suppliers for quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the average daily sales per quartile
        /// </summary>
        /// <returns>The average daily sales per quartile</returns>
        [HttpGet("average-daily-sales-per-quartile")]
        public IActionResult GetAVGDailySalesPerQuartile()
        {
            try
            {
                // Retrieve the ranking of the most sold products per quartile
                var AverageDailySalesPerQuartile = _informationService.GetAverageDailySalesPerQuartile();

                // Return HTTP 200 OK response with the most sold products data
                return Ok(AverageDailySalesPerQuartile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the avg daily sales per quartile.");
                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the avg daily sales per quartile.");
            }
        }

        /// <summary>
        /// HTTP GET method to retrieve the Monthly Sales Mode By Quartile
        /// </summary>
        /// <returns>The Monthly Sales Mode By Quartile</returns>
        [HttpGet("month-sales-mode-by-quartile")]
        public IActionResult GetMonthlySalesModeByQuartile()
        {
            try
            {
                // Retrieve the ranking of the most sold products per quartile
                var monthSalesMode = _informationService.GetMonthSalesModeByQuartile();

                // Return HTTP 200 OK response with the most sold products data
                return Ok(monthSalesMode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the montly sales mode by quartile.");
                // Return HTTP 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the montly sales mode by quartile.");
            }
        }
    }
}