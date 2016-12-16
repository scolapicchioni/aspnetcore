using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace PhotoSharingApplication.ActionFilters
{
    public class ValueReporter : ActionFilterAttribute
    {
        private ILogger _logger;
        public ValueReporter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ValueReporter ActionFilter");
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            logValues(filterContext.RouteData);
        }

        private void logValues(RouteData routeData)
        {
            var controller = routeData.Values["controller"];
            var action = routeData.Values["action"];
            _logger.LogInformation($"Controller: {controller}; Action: {action}");

            foreach (var item in routeData.Values)
            {
                _logger.LogInformation($">> Key: {item.Key}; Value {item.Value}");
            }
        }
    }
}