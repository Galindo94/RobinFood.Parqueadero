namespace Applicacion.Parqueadero.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Applicacion.Transferencias.WebApi.Handlers;
    using Applicacion.Transferencias.WebApi.Models;
    using Common.Utils.Resources;
    using Domain.Service.DTO.Vehicle;
    using Domain.Service.Services;
    using Domain.Service.Services.Abstract;
    using Domain.Service.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using static Common.Utils.Enums.Enums;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    [TypeFilter(typeof(CacheGlobalVariables))]
    public class GeneralController : ControllerBase
    {
        #region Atributes

        private readonly IAmountService amountService;

        #endregion Atributes

        #region Builder

        public GeneralController(IAmountService _amountService)
        {
            amountService = _amountService;
        }

        #endregion Builder

        /// <summary>
        /// Create amount to pay for a Vehicle Type
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="VehicleType"></param>
        /// <returns></returns>
        [HttpPost("CreateAmount")]
        public IActionResult CreateAmount(decimal Amount, VehicleType VehicleType)
        {
            IActionResult oResponse;
            ResponseModel<object> oResponseModel;
            bool oResult = amountService.CreateAmount(Amount, VehicleType);

            oResponseModel = new ResponseModel<object>()
            {
                IsSuccess = oResult,
                Messages = oResult ? GeneralMessages.ItemInserted : GeneralMessages.ItemNoInserted,
                Result = null
            };

            oResponse = Ok(oResponseModel);

            return oResponse;
        }
    }
}
