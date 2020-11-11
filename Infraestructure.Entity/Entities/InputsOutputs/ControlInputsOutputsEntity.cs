namespace Infraestructure.Entity.Entities.InputsOutputs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [Table("ControlInputsOutputs", Schema = "InputsOutputs")]
    public class ControlInputsOutputsEntity : AuditEntity
    {
        [Key]
        public int ControlInputsOutputsId { get; set; }

        [Column("VehiclePlate", TypeName = "varchar(6)")]
        public string VehiclePlate { get; set; }

        [Column("VehicleType", TypeName = "int")]
        public int VehicleType { get; set; }

        [Column("EntryTime", TypeName = "datetime2(7)")]
        public DateTime EntryTime { get; set; }

        [Column("DepartureTime", TypeName = "datetime2(7)")]
        public DateTime? DepartureTime { get; set; }

        [Column("TotalMinutesOfStay", TypeName = "decimal(18,2)")]
        public decimal TotalMinutesOfStay { get; set; }
    }
}
