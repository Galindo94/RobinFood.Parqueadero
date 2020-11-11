namespace Domain.Service.Class
{
    using Common.Utils.Constant;
    using Common.Utils.Enums;
    using Common.Utils.Excepcions;
    using Common.Utils.Resources;
    using Domain.Service.Services.Abstract;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Infraestructure.Entity.Entities.InputsOutputs;
    using Infraestructure.Entity.Entities.Vehicle;
    using System;
    using static Common.Utils.Enums.Enums;

    class PostPaidResidentVehicle : AVehicleBase
    {
        public readonly IUnitOfWork unitOfWork;

        public PostPaidResidentVehicle(VehicleEntity _vehicle, IUnitOfWork iUnitOfWork) : base(_vehicle)
        {
            unitOfWork = iUnitOfWork;
        }

        public override decimal RegisterOutPut(ControlInputsOutputsEntity controlInputsOutputs)
        {
            VehicleEntity oVehicle = unitOfWork.
                            VehicleRepository.
                            FirstOrDefault(g => g.VehiclePlate.ToUpper().Equals(controlInputsOutputs.VehiclePlate.ToUpper())
                                             && g.VehicleType.Equals((int)VehicleType.PostPaidResident));

            if (oVehicle == null)
                throw new BusinessExeption(string.Format(GeneralMessages.VehicleAndTypeVehicleNotFound,
                                                           controlInputsOutputs.VehiclePlate.ToUpper(),
                                                           VehicleType.PostPaidResident.GetDisplayName()));

            controlInputsOutputs.DepartureTime = DateTime.Now;
            controlInputsOutputs.TotalMinutesOfStay = (decimal)(DateTime.Now - controlInputsOutputs.EntryTime).TotalMinutes;
            unitOfWork.ControlInputsOutputsRepository.Update(controlInputsOutputs);

            oVehicle.TotalMinutesOfStay += controlInputsOutputs.TotalMinutesOfStay;
            oVehicle.ModificationDate = DateTime.Now;
            oVehicle.ModificationUser = UsersParkingLot.System;
            unitOfWork.VehicleRepository.Update(oVehicle);

            return 0;
        }

        public override decimal GetAmount(VehicleEntity oVehicle)
        {
            throw new BusinessExeption(string.Format(GeneralMessages.InvalidVehicleTypeForPaymentModule,
                VehicleType.Resident.GetDisplayName(),
                VehicleType.PostPaidResident.GetDisplayName()));
        }
    }
}
