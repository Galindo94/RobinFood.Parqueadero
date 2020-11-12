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
    [TypeFilter(typeof(CacheGlobalVariables))]
    public class ResidentPaymentsController : ControllerBase
    {
        #region Atributes

        private readonly IResidentPaymentsService residentPaymentsService;

        #endregion Atributes

        #region Builder

        public ResidentPaymentsController(IResidentPaymentsService _residentPaymentsService)
        {
            residentPaymentsService = _residentPaymentsService;
        }

        #endregion Builder


        /// <summary>
        /// Record resident vehicle payment
        /// </summary>
        /// <param name="VehiclePlate"></param>
        /// <returns></returns>
        [HttpPost("GetAmount")]
        public IActionResult GetAmount(string VehiclePlate)
        {
            IActionResult oResponse;
            ResponseModel<object> oResponseModel;
            GetAmountResponseDto oResult = residentPaymentsService.GetAmount(VehiclePlate);

            oResponseModel = new ResponseModel<object>()
            {
                IsSuccess = true,
                Messages = string.Format(GeneralMessages.AmountToPay, VehiclePlate, oResult.Amount),
                Result = null
            };

            oResponse = Ok(oResponseModel);

            return oResponse;
        }


        [HttpPost("InsertPayment")]
        public IActionResult InsertPayment(InsertPaymentRequestDto insertPaymentRequestDto)
        {
            IActionResult oResponse;
            ResponseModel<object> oResponseModel;
            CreateVehicleResponseDto oResult = residentPaymentsService.InsertPayment(insertPaymentRequestDto);

            oResponseModel = new ResponseModel<object>()
            {
                IsSuccess = oResult.Inserted,
                Messages = oResult.Message,
                Result = null
            };

            oResponse = Ok(oResponseModel);

            return oResponse;
        }

    }
}
