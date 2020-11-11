namespace Domain.Service.DTO.Vehicle
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RegisterOutPutResponseDto : CreateVehicleResponseDto
    {
        public decimal AmountToBePaid { get; set; }
        
    }
}
