namespace Infraestructure.Entity.Entities.Vehicle
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [Table("PaymentVehicle", Schema = "Vehicle")]
    public class PaymentVehicleEntity : AuditEntity
    {
        [Key]
        [Column("PaymentVehicleId", TypeName = "int")]
        public int PaymentVehicleId { get; set; }

        [Column("CutoffDate", TypeName = "datetime2(7)")]
        public DateTime PaymentDate { get; set; }

        [Column("TotalMinutesOfStay", TypeName = "decimal(18,2)")]
        public decimal PaymentValue { get; set; }

        [ForeignKey("VehicleEntity")]
        [Column("VehicleId", TypeName = "int")]
        public int VehicleId { get; set; }

        public VehicleEntity VehicleEntity { get; set; }

    }
}
