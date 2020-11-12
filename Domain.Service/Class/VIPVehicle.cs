namespace Domain.Service.Class
{
    using Common.Utils.Constant;
    using Common.Utils.Enums;
    using Common.Utils.Excepcions;
    using Common.Utils.Resources;
    using Domain.Service.DTO.General;
    using Domain.Service.DTO.Vehicle;
    using Domain.Service.Services.Abstract;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Infraestructure.Entity.Entities.General;
    using Infraestructure.Entity.Entities.InputsOutputs;
    using Infraestructure.Entity.Entities.Vehicle;
    using System;
    using static Common.Utils.Enums.Enums;

    class VIPVehicle : AVehicleBase
    {
        public readonly IUnitOfWork unitOfWork;

        public VIPVehicle(VehicleEntity _vehicle, IUnitOfWork iUnitOfWork) : base(_vehicle)
        {
            unitOfWork = iUnitOfWork;
        }

        public override decimal RegisterOutPut(ControlInputsOutputsEntity controlInputsOutputs)
        {
            AmountEntity oAmount = unitOfWork
                .AmountRepository
                .LastOrDefault(g => g.VehicleType.Equals((int)VehicleType.VIP));

            if (oAmount == null)
                throw new BusinessExeption(string.Format(GeneralMessages.AmountNotFound, VehicleType.VIP.GetDisplayName()));


            VehicleEntity oVehicle = unitOfWork.
                VehicleRepository.
                FirstOrDefault(g => g.VehiclePlate.ToUpper().Equals(controlInputsOutputs.VehiclePlate.ToUpper())
                                 && g.VehicleType.Equals((int)VehicleType.VIP));

            if (oVehicle == null)
                throw new BusinessExeption(string.Format(GeneralMessages.VehicleAndTypeVehicleNotFound,
                                                           controlInputsOutputs.VehiclePlate.ToUpper(),
                                                           VehicleType.VIP.GetDisplayName()));

            controlInputsOutputs.DepartureTime = DateTime.Now;
            controlInputsOutputs.TotalMinutesOfStay = (decimal)(DateTime.Now - controlInputsOutputs.EntryTime).TotalMinutes;
            unitOfWork.ControlInputsOutputsRepository.Update(controlInputsOutputs);

            oVehicle.TotalMinutesOfStay += controlInputsOutputs.TotalMinutesOfStay;
            oVehicle.ModificationDate = DateTime.Now;
            oVehicle.ModificationUser = UsersParkingLot.System;
            unitOfWork.VehicleRepository.Update(oVehicle);

            return controlInputsOutputs.TotalMinutesOfStay * oAmount.Amount;
        }

        public override decimal GetAmount(VehicleEntity oVehicle)
        {
            throw new BusinessExeption(string.Format(GeneralMessages.InvalidVehicleTypeForPaymentModule,
                VehicleType.Resident.GetDisplayName(),
                VehicleType.PostPaidResident.GetDisplayName()));
        }

        public override CreateVehicleResponseDto InsertPayment(GetAmountResponseDto getAmountResponseDto, decimal PaymentValue)
        {
            throw new BusinessExeption(string.Format(GeneralMessages.InvalidVehicleTypeForPaymentModule,
                           VehicleType.Resident.GetDisplayName(),
                           VehicleType.PostPaidResident.GetDisplayName()));
        }
    }
}
