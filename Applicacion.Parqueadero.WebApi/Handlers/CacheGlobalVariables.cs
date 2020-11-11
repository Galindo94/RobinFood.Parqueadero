namespace Applicacion.Transferencias.WebApi.Handlers
{
    using Common.Utils.Constant;
    using Common.Utils.Utils.Interface;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    [ExcludeFromCodeCoverage]
    public class CacheGlobalVariables : IActionFilter
    {
        private readonly IUtils _utils;

        public CacheGlobalVariables( IUtils utils)
        {
            _utils = utils;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            

           
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}