namespace Domain.Service.Services
{
    using Common.Utils.Excepcions;
    using Common.Utils.Resources;
    using Domain.Service.Class;
    using Domain.Service.DTO.General;
    using Domain.Service.DTO.Vehicle;
    using Domain.Service.Services.Abstract;
    using Domain.Service.Services.Interface;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Infraestructure.Entity.Entities.Vehicle;
    using System;
    using static Common.Utils.Enums.Enums;

    public class ResidentPaymentsService : IResidentPaymentsService
    {
        #region Members

        public readonly IUnitOfWork unitOfWork;
        private readonly IVehicleService vehicleService;

        #endregion Members

        #region Builder

        public ResidentPaymentsService(IUnitOfWork _unitOfWork,
            IVehicleService _vehicleService)
        {
            unitOfWork = _unitOfWork;
            vehicleService = _vehicleService;
        }

        #endregion Builder       

        public GetAmountResponseDto GetAmount(string VehiclePlate)
        {
            GetAmountResponseDto oResponse = new GetAmountResponseDto
            {
                Vehicle = vehicleService.GetVehicle(VehiclePlate)
            };

            if (oResponse.Vehicle == null)
                throw new BusinessExeption(string.Format(GeneralMessages.VehicleNotFound, VehiclePlate));

            AVehicleBase aVehicle = GetVehicleBase(oResponse.Vehicle, (VehicleType)Enum.ToObject(typeof(VehicleType), oResponse.Vehicle.VehicleType));

            oResponse.Amount = aVehicle.GetAmount(oResponse.Vehicle);

            return oResponse;
        }

        public CreateVehicleResponseDto InsertPayment(InsertPaymentRequestDto insertPaymentRequestDto)
        {
            if (insertPaymentRequestDto.PaymentValue == 0)
                throw new BusinessExeption(GeneralMessages.PaymentGreaterThanZero);

            GetAmountResponseDto oResponseGetAmount = GetAmount(insertPaymentRequestDto.VehiclePlate);

            if (oResponseGetAmount.Amount == 0)
                throw new BusinessExeption(string.Format(GeneralMessages.VehicleADay, oResponseGetAmount.Vehicle.VehiclePlate, oResponseGetAmount.Vehicle.CutoffDate.Date));

            if (oResponseGetAmount.Amount > insertPaymentRequestDto.PaymentValue)
                throw new BusinessExeption(string.Format(GeneralMessages.LowerPaymentValue, oResponseGetAmount.Vehicle.VehiclePlate, oResponseGetAmount.Amount));


            AVehicleBase aVehicle = GetVehicleBase(oResponseGetAmount.Vehicle, (VehicleType)Enum.ToObject(typeof(VehicleType), oResponseGetAmount.Vehicle.VehicleType));

            return aVehicle.InsertPayment(oResponseGetAmount, insertPaymentRequestDto.PaymentValue);
        }

        private AVehicleBase GetVehicleBase(VehicleEntity oVehicle, VehicleType vehicleType)
        {
            if (!Enum.IsDefined(typeof(VehicleType), vehicleType))
                throw new BusinessExeption(GeneralMessages.VehicleTypeNotSupported);

            AVehicleBase aVehicle;

            switch (vehicleType)
            {
                case VehicleType.NotType:
                    aVehicle = null;
                    break;

                case VehicleType.VIP:
                    aVehicle = new VIPVehicle(oVehicle, unitOfWork);
                    break;
                case VehicleType.Resident:
                    aVehicle = new ResidentVehicle(oVehicle, unitOfWork);
                    break;
                case VehicleType.PostPaidResident:
                    aVehicle = new PostPaidResidentVehicle(oVehicle, unitOfWork);
                    break;
                case VehicleType.NotResident:
                    aVehicle = new NoResidentVehicle(oVehicle, unitOfWork);
                    break;

                default:
                    aVehicle = null;
                    break;
            }

            return aVehicle;
        }
    }
}
