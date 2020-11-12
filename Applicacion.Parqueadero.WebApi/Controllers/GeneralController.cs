namespace Applicacion.Parqueadero.WebApi.Controllers
{
    using Applicacion.Transferencias.WebApi.Handlers;
    using Applicacion.Transferencias.WebApi.Models;
    using Common.Utils.Resources;
    using Domain.Service.DTO.General;
    using Domain.Service.DTO.Vehicle;
    using Domain.Service.Services;
    using Domain.Service.Services.Abstract;
    using Domain.Service.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Common.Utils.Enums.Enums;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
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
        /// <param name="oRequest"></param>
        /// <returns></returns>
        [HttpPost("CreateAmount")]
        public IActionResult CreateAmount(CreateAmountRequestDto oRequest)
        {
            IActionResult oResponse;
            ResponseModel<object> oResponseModel;
            bool oResult = amountService.CreateAmount(oRequest.Amount, oRequest.VehicleType);

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
