using System;
using System.Collections.Generic;
using System.Text;
using static Common.Utils.Enums.Enums;

namespace Domain.Service.DTO.General
{
    public class CreateAmountRequestDto
    {
        public decimal Amount { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
