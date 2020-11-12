namespace Domain.Service.Services.Abstract
{
    using Domain.Service.DTO.General;
    using Domain.Service.DTO.Vehicle;
    using Infraestructure.Entity.Entities.InputsOutputs;
    using Infraestructure.Entity.Entities.Vehicle;

    abstract class AVehicleBase
    {
        protected VehicleEntity vehicle;

        public AVehicleBase(VehicleEntity _vehicle)
        {
            vehicle = _vehicle;
        }

        public abstract decimal RegisterOutPut(ControlInputsOutputsEntity controlInputsOutputs);

        public abstract decimal GetAmount(VehicleEntity oVehicle);

        public abstract CreateVehicleResponseDto InsertPayment(GetAmountResponseDto getAmountResponseDto, decimal PaymentValue);


    }
}
