namespace Infraestructure.Core.UnitOfWork.Interface
{
    using Infraestructure.Core.Repository;
    using Infraestructure.Entity.Entities.General;
    using Infraestructure.Entity.Entities.InputsOutputs;
    using Infraestructure.Entity.Entities.Vehicle;
    using System.Collections.Generic;

    public interface IUnitOfWork
    {
        Repository<VehicleEntity> VehicleRepository { get; }
        Repository<PaymentVehicleEntity> PaymentVehicleRepository { get; }
        Repository<ControlInputsOutputsEntity> ControlInputsOutputsRepository { get; }
        Repository<AmountEntity> AmountRepository { get; }

        int Save();

        IEnumerable<T> ExecuteSqlStoreProcedure<T>(string storeProcedureName, Dictionary<string, object> parameters);
    }
}