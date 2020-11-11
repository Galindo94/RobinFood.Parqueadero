namespace Applicacion.Transferencias.WebApi.Handlers
{
    using Applicacion.Transferencias.WebApi.Models;
    using Applicacion.Transferencias.WebApi.Models.General;
    using Common.Utils.Constant;
    using Common.Utils.Enums;
    using Common.Utils.Excepcions;
    using Common.Utils.Resources;
    using Common.Utils.Utils;
    using Common.Utils.Utils.Interface;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    [ExcludeFromCodeCoverage]
    public class CustomExceptionHandler : ExceptionFilterAttribute
    {
      

        public CustomExceptionHandler()
        {
        }

        /// <summary>
        /// Metodo encargado de capturar todas las Excepciones del proyecto,
        /// Se debe agregar el decorador a cada controller [TypeFilter(typeof(CustomExceptionHandler))]
        /// </summary>
        /// <param name="oException"> Parametro donde queda capturada la Exception </param>
        public override void OnException(ExceptionContext context)
        {
            HttpResponseException oResponseExeption = new HttpResponseException();
            ResponseModel<string> oResponse = new ResponseModel<string>()
            {
                IsSuccess = false,
                Result = JsonConvert.SerializeObject(context.Exception)
            };

            if (context.Exception is BusinessExeption)
            {
                oResponseExeption.Status = StatusCodes.Status400BadRequest;
                oResponse.Messages = context.Exception.Message;
                context.ExceptionHandled = true;
            }
            else
            {
                if (context.Exception is Exception)
                {
                    oResponseExeption.Status = StatusCodes.Status500InternalServerError;
                    oResponse.Messages = GeneralMessages.Error500;
                }
                context.ExceptionHandled = true;
            }

            context.Result = new ObjectResult(oResponseExeption.Value)
            {
                StatusCode = oResponseExeption.Status,
                Value = oResponse
            };

            if (oResponseExeption.Status == StatusCodes.Status500InternalServerError)
                context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = GeneralMessages.Error500;
        }

        
    }
}