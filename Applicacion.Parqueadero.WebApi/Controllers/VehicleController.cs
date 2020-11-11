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
    public class VehicleController : ControllerBase
    {
        #region Atributes

        private readonly IVehicleService vehicleService;

        #endregion Atributes

        #region Builder

        public VehicleController(IVehicleService _vehicleService)
        {
            vehicleService = _vehicleService;
        }

        #endregion Builder

        /// <summary>
        /// Create Vehicle
        /// </summary>
        /// <param name="VehiclePlate"></param>
        /// <param name="VehicleType"></param>
        /// <returns></returns>
        [HttpPost("CreateVehicle")]
        public IActionResult CreateVehicle(string VehiclePlate, VehicleType VehicleType)
        {
            IActionResult oResponse;
            ResponseModel<CreateVehicleResponseDto> oResponseModel;
            CreateVehicleResponseDto oResult = vehicleService.CreateVehicle(VehiclePlate, VehicleType);

            oResponseModel = new ResponseModel<CreateVehicleResponseDto>()
            {
                IsSuccess = oResult.Inserted,
                Messages = oResult.Message,
                Result = null
            };

            oResponse = Ok(oResponseModel);

            return oResponse;
        }

        /// <summary>
        /// Register the entry of a vehicle
        /// </summary>
        /// <param name="VehiclePlate"></param>
        /// <returns></returns>
        [HttpPost("RegisterInput")]
        public IActionResult RegisterInput(string VehiclePlate)
        {
            IActionResult oResponse;
            ResponseModel<CreateVehicleResponseDto> oResponseModel;
            CreateVehicleResponseDto oResult = vehicleService.RegisterInput(VehiclePlate);

            oResponseModel = new ResponseModel<CreateVehicleResponseDto>()
            {
                IsSuccess = oResult.Inserted,
                Messages = oResult.Message,
                Result = null
            };

            oResponse = Ok(oResponseModel);

            return oResponse;
        }

        /// <summary>
        /// Register the departure of a vehicle
        /// </summary>
        /// <param name="VehiclePlate"></param>
        /// <returns></returns>
        [HttpPost("RegisterOutPut")]
        public IActionResult RegisterOutPut(string VehiclePlate)
        {
            IActionResult oResponse;
            ResponseModel<object> oResponseModel;
            RegisterOutPutResponseDto oResult = vehicleService.RegisterOutPut(VehiclePlate);

            oResponseModel = new ResponseModel<object>()
            {
                IsSuccess = oResult.Inserted,
                Messages = oResult.Inserted ? string.Format(GeneralMessages.ValueToPayForTheStay, oResult.AmountToBePaid) : oResult.Message,
                Result = null
            };

            oResponse = Ok(oResponseModel);

            return oResponse;
        }

    }
}
