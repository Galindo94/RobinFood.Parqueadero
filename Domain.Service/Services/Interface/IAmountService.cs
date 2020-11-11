namespace Domain.Service.Services.Interface
{
    using Infraestructure.Entity.Entities.General;
    using static Common.Utils.Enums.Enums;

    public interface IAmountService
    {
        bool CreateAmount(decimal Amount, VehicleType VehicleType);
        AmountEntity GetVigentAmount(VehicleType vehicleType);
    }
}
