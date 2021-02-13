using System;
using ASM.Util.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ASM.Api.Middleware
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, Models.Response<ApiError>> AddResponseDetails { get; set; }

        public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
    }
}
