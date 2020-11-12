namespace Domain.Service.Services.Interface
{
    using Domain.Service.DTO.General;
    using Domain.Service.DTO.Vehicle;

    public interface IResidentPaymentsService
    {
        GetAmountResponseDto GetAmount(string VehiclePlate);
        CreateVehicleResponseDto InsertPayment(InsertPaymentRequestDto insertPaymentRequestDto);
    }
}
