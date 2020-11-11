namespace Domain.Service.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IResidentPaymentsService
    {
        decimal GetAmount(string VehiclePlate);
    }
}
