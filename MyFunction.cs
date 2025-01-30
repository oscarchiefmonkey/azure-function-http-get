using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;

namespace FunctionApp12
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        public Function1 (ILogger<Function1> logger)
        {
            _logger = logger;
        }
        [Function("Function1")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger?.LogCritical("Critical log");
            _logger?.LogWarning("Warning log");
            _logger?.LogError("Error Log");
            _logger?.LogInformation("Information log");

            var responseMessage = "Welcome to Azure Functions";

           return new OkObjectResult(responseMessage);
        }
    }
}
