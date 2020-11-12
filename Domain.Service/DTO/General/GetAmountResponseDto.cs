namespace Domain.Service.DTO.General
{
    using Infraestructure.Entity.Entities.Vehicle;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GetAmountResponseDto
    {
        public decimal Amount { get; set; }
        public VehicleEntity Vehicle { get; set; }
    }
}
