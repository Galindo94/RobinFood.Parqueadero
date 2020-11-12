namespace Domain.Service.Class
{
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

    class NoResidentVehicle : AVehicleBase
    {
        public readonly IUnitOfWork unitOfWork;

        public NoResidentVehicle(VehicleEntity _vehicle, IUnitOfWork iUnitOfWork) : base(_vehicle)
        {
            unitOfWork = iUnitOfWork;
        }

        public override decimal RegisterOutPut(ControlInputsOutputsEntity controlInputsOutputs)
        {
            AmountEntity oAmount = unitOfWork
                .AmountRepository
                .LastOrDefault(g => g.VehicleType.Equals((int)VehicleType.NotResident));

            if (oAmount == null)
                throw new BusinessExeption(string.Format(GeneralMessages.AmountNotFound, VehicleType.NotResident.GetDisplayName()));

            controlInputsOutputs.DepartureTime = DateTime.Now;
            controlInputsOutputs.TotalMinutesOfStay = (decimal)(DateTime.Now - controlInputsOutputs.EntryTime).TotalMinutes;
            unitOfWork.ControlInputsOutputsRepository.Update(controlInputsOutputs);

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
