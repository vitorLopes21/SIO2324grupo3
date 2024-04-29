namespace Sio2324_Grupo_03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CInformationExpert : ControllerBase
    {
        private readonly IInformationExpert _informationService;

        public CInformationExpert(IInformationExpert informationService)
        {
            _informationService = informationService;
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


        // Add other action methods for additional endpoints
    }
}
