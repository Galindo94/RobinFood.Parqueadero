using Common.Utils.Resources;

namespace Domain.Service.DTO.Vehicle
{
    public class CreateVehicleResponseDto
    {
        public bool Inserted { get; set; }
        public string Message { get; set; }

        public CreateVehicleResponseDto()
        {
            Inserted = false;
            Message = GeneralMessages.ItemNoInserted;
        }
    }
}
