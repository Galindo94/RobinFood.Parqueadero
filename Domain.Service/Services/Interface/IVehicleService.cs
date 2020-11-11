namespace Domain.Service.Services.Interface
{
    using Domain.Service.DTO.Vehicle;
    using Infraestructure.Entity.Entities.Vehicle;
    using static Common.Utils.Enums.Enums;

    public interface IVehicleService
    {
        CreateVehicleResponseDto CreateVehicle(string VehiclePlate, VehicleType VehicleType);
        CreateVehicleResponseDto RegisterInput(string VehiclePlate);
        RegisterOutPutResponseDto RegisterOutPut(string VehiclePlate);
        VehicleEntity GetVehicle(string VehiclePlate);

    }
}
