namespace DecimalDemoProject.Controllers
{
    using DecimalDemoProject.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class DecimalValuesController : ControllerBase
    {
        private readonly IDecimalValuesService _decimalValuesService;

        public DecimalValuesController(IDecimalValuesService decimalValuesService)
        {
            _decimalValuesService = decimalValuesService;
        }

        [HttpGet]
        public async Task<ResultModel> Get([FromQuery]DecimalValuesRequest request, CancellationToken cancellationToken)
        {
            var result = await _decimalValuesService.CalculationsAsync(request, cancellationToken);
            return result;
        }
    }
}
