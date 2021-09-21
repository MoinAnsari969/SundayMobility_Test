using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsCrudApi.Filters
{
    public class StudentRequestResponseLoggerFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public StudentRequestResponseLoggerFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<StudentRequestResponseLoggerFilter>();
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Request Begin");
            _logger.LogInformation("Request Path : {0}", context.HttpContext.Request.Path.ToString());
            _logger.LogInformation("Request Method : {0}", context.HttpContext.Request.Method.ToString());
            if (context.ActionArguments.Count >1 )
            {
                var requestBodyData = context.ActionArguments["student"];
                _logger.LogInformation("Request Body : {0}", JsonConvert.SerializeObject(requestBodyData));
            }
            _logger.LogInformation("Request End");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Response Begin");
            _logger.LogInformation("Response Status Code : {0}", context.HttpContext.Response.StatusCode.ToString());
            var resultType=context.Result.GetType();
            if (context.Result is ObjectResult objectResult)
            {
                var responseBodyData = JsonConvert.SerializeObject(objectResult.Value);
                _logger.LogInformation("Response Body : {0}", JsonConvert.SerializeObject(responseBodyData));
            }
            _logger.LogInformation("Response End");
        }
    }
}
