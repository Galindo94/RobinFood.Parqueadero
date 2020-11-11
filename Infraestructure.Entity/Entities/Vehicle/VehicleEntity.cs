namespace Infraestructure.Entity.Entities.Vehicle
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [Table("Vehicle", Schema = "Vehicle")]
    public class VehicleEntity : AuditEntity
    {
        [Key]
        [Column("VehicleId", TypeName = "int")]
        public int VehicleId { get; set; }

        [Column("VehiclePlate", TypeName = "varchar(6)")]
        public string VehiclePlate { get; set; }

        [Column("VehicleType", TypeName = "int")]
        public int VehicleType { get; set; }

        [Column("CutoffDate", TypeName = "datetime2(7)")]
        public DateTime CutoffDate { get; set; }

        [Column("TotalMinutesOfStay", TypeName = "decimal(18,2)")]
        public decimal TotalMinutesOfStay { get; set; }


        #region FK
        public virtual ICollection<PaymentVehicleEntity> PaymentVehicleEntities { get; set; }

        #endregion FK
    }
}
