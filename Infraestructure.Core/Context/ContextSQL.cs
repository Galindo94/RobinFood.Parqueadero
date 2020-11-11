namespace Infraestructure.Core.Context.SQLServer
{
    using Infraestructure.Entity.Entities.General;
    using Infraestructure.Entity.Entities.InputsOutputs;
    using Infraestructure.Entity.Entities.Vehicle;
    using Microsoft.EntityFrameworkCore;

    public class ContextSql : DbContext
    {
        public ContextSql(DbContextOptions dbContextOptions)
           : base(dbContextOptions)
        {
        }

        #region DbSet Entities     

        public DbSet<VehicleEntity> VehicleEntity { get; set; }
        public DbSet<PaymentVehicleEntity> PaymentVehicleEntity { get; set; }
        public DbSet<ControlInputsOutputsEntity> ControlInputsOutputsEntity { get; set; }
        public DbSet<AmountEntity> AmountEntity { get; set; }

        #endregion DbSet Entities

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}