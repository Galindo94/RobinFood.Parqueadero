namespace Infraestructure.Entity.Entities
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class AuditEntity
    {
        public DateTime? CreationDate { get; set; }

        public string CreationUser { get; set; }

        public DateTime? ModificationDate { get; set; }

        public string ModificationUser { get; set; }
    }
}