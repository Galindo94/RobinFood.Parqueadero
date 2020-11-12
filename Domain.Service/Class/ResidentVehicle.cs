namespace Domain.Service.Class
{
    using Common.Utils.Constant;
    using Common.Utils.Enums;
    using Common.Utils.Excepcions;
    using Common.Utils.Resources;
    using DocumentFormat.OpenXml.Office2010.ExcelAc;
    using Domain.Service.DTO.General;
    using Domain.Service.DTO.Vehicle;
    using Domain.Service.Services.Abstract;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Infraestructure.Entity.Entities.General;
    using Infraestructure.Entity.Entities.InputsOutputs;
    using Infraestructure.Entity.Entities.Vehicle;
    using System;
    using System.Collections.Generic;
    using static Common.Utils.Enums.Enums;

    class ResidentVehicle : AVehicleBase
    {
        public readonly IUnitOfWork unitOfWork;

        public ResidentVehicle(VehicleEntity _vehicle, IUnitOfWork iUnitOfWork) : base(_vehicle)
        {
            unitOfWork = iUnitOfWork;
        }

        public override decimal RegisterOutPut(ControlInputsOutputsEntity controlInputsOutputs)
        {
            VehicleEntity oVehicle = unitOfWork.
                VehicleRepository.
                FirstOrDefault(g => g.VehiclePlate.ToUpper().Equals(controlInputsOutputs.VehiclePlate.ToUpper())
                                 && g.VehicleType.Equals((int)VehicleType.Resident));

            if (oVehicle == null)
                throw new BusinessExeption(string.Format(GeneralMessages.VehicleAndTypeVehicleNotFound,
                                                           controlInputsOutputs.VehiclePlate.ToUpper(),
                                                           VehicleType.Resident.GetDisplayName()));

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
            AmountEntity oAmount = GetVigentAmount(VehicleType.Resident);
            if (oAmount == null)
                throw new BusinessExeption(string.Format(GeneralMessages.AmountNotFound, VehicleType.Resident.GetDisplayName()));

            decimal Amount = 0;
            int Monts = GetMonts(oVehicle.CutoffDate);
            int Payments = oVehicle.PaymentVehicleEntities.Count;
            int diference = Monts - Payments;

            if (diference > 0)
                Amount = diference * oAmount.Amount;

            return Amount;
        }

        public override CreateVehicleResponseDto InsertPayment(GetAmountResponseDto getAmountResponseDto, decimal PaymentValue)
        {
            VehicleEntity oVehicle = getAmountResponseDto.Vehicle;
            oVehicle.PaymentVehicleEntities = new List<PaymentVehicleEntity>
            {
                new PaymentVehicleEntity()
                {
                    PaymentDate = DateTime.Now,
                    PaymentValue = PaymentValue,
                    CreationDate = DateTime.Now,
                    CreationUser = UsersParkingLot.System
                }
            };

            unitOfWork.VehicleRepository.Update(oVehicle);
            bool inserted = unitOfWork.Save() > 0;

            CreateVehicleResponseDto oResponse = new CreateVehicleResponseDto
            {
                Inserted = inserted,
                Message = inserted ? string.Format(GeneralMessages.RegisteredPayment, getAmountResponseDto.Vehicle.VehiclePlate) : GeneralMessages.ItemNoInserted
            };

            return oResponse;
        }

        private AmountEntity GetVigentAmount(VehicleType vehicleType)
        {
            return unitOfWork
               .AmountRepository
               .LastOrDefault(g => g.VehicleType.Equals((int)vehicleType));
        }

        private static int GetMonts(DateTime CutoffDate)
        {
            return (int)(((DateTime.Now - CutoffDate).TotalDays / 365.25) * 12);
        }
    }
}
