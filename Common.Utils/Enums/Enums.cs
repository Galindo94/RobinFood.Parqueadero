namespace Common.Utils.Enums
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class Enums
    {
        public enum VehicleType
        {
            [Display(Name = "Sin Tipificar")]
            NotType = 0,

            [Display(Name = "VIP")]
            VIP = 1,

            [Display(Name = "Residente")]
            Resident = 2,

            [Display(Name = "Residente PostPago")]
            PostPaidResident = 3,

            [Display(Name = "No Residente")]
            NotResident = 4
        }

    }
}