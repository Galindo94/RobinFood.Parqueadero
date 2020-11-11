namespace Domain.Service.Services
{
    using Common.Utils.Enums;
    using Common.Utils.Excepcions;
    using Common.Utils.Resources;
    using Domain.Service.Class;
    using Domain.Service.Services.Abstract;
    using Domain.Service.Services.Interface;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Infraestructure.Entity.Entities.General;
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

        public decimal GetAmount(string VehiclePlate)
        {
            VehicleEntity oVehicle = vehicleService.GetVehicle(VehiclePlate);

            if (oVehicle == null)
                throw new BusinessExeption(string.Format(GeneralMessages.VehicleNotFound, VehiclePlate));

            AVehicleBase aVehicle = GetVehicleBase(oVehicle, (VehicleType)Enum.ToObject(typeof(VehicleType), oVehicle.VehicleType));

            return aVehicle.GetAmount(oVehicle);
        }

        private AVehicleBase GetVehicleBase(VehicleEntity oVehicle, VehicleType vehicleType)
        {
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
