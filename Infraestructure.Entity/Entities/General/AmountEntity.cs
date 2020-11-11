namespace Infraestructure.Entity.Entities.General
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [Table("Amount", Schema = "General")]
    public class AmountEntity : AuditEntity
    {
        [Key]
        public int AmountId { get; set; }

        [Column("Amount", TypeName = "decimal(18,0)")]
        public decimal Amount { get; set; }

        [Column("VehicleType", TypeName = "int")]
        public int VehicleType { get; set; }
    }
}
