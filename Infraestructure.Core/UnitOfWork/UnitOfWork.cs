namespace Infraestructure.Core.UnitOfWork
{
    using Common.Utils.Utils.Interface;
    using Infraestructure.Core.Context.SQLServer;
    using Infraestructure.Core.Repository;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Infraestructure.Entity.Entities.General;
    using Infraestructure.Entity.Entities.InputsOutputs;
    using Infraestructure.Entity.Entities.Vehicle;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {
        #region Attributes

        private readonly ContextSql _context;

        #endregion Attributes

        #region Constructor

        public UnitOfWork(ContextSql context)
        {
            this._context = context;
        }

        #endregion Constructor

        #region Attributes
        private Repository<VehicleEntity> vehicleRepository;
        private Repository<PaymentVehicleEntity> paymentVehicleRepository;
        private Repository<ControlInputsOutputsEntity> controlInputsOutputsRepository;
        private Repository<AmountEntity> amountRepository;


        #endregion Attributes

        #region Members

        public Repository<VehicleEntity> VehicleRepository
        {
            get
            {
                if (this.vehicleRepository == null)
                    this.vehicleRepository = new Repository<VehicleEntity>(_context);

                return vehicleRepository;
            }
        }

        public Repository<PaymentVehicleEntity> PaymentVehicleRepository
        {
            get
            {
                if (this.paymentVehicleRepository == null)
                    this.paymentVehicleRepository = new Repository<PaymentVehicleEntity>(_context);

                return paymentVehicleRepository;
            }
        }

        public Repository<ControlInputsOutputsEntity> ControlInputsOutputsRepository
        {
            get
            {
                if (this.controlInputsOutputsRepository == null)
                    this.controlInputsOutputsRepository = new Repository<ControlInputsOutputsEntity>(_context);

                return controlInputsOutputsRepository;
            }
        }

        public Repository<AmountEntity> AmountRepository
        {
            get
            {
                if (this.amountRepository == null)
                    this.amountRepository = new Repository<AmountEntity>(_context);

                return amountRepository;
            }
        }

        #endregion Members

        public int Save()
        {
            int result = 0;
            EntityState state = EntityState.Unchanged;
            Dictionary<string, List<object>> changes = GetChanged(ref state);
            result = _context.SaveChanges();
            return result;
        }

        /// <summary>
        /// Execute store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedureName"></param>
        /// <param name="parameters"></param>
        /// <returns>the fields of the DTO must be the same fields of the query</returns>
        public IEnumerable<T> ExecuteSqlStoreProcedure<T>(string storeProcedureName, Dictionary<string, object> parameters)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedureName;

                Dictionary<Type, DbType> typeMap = GetTypes();

                foreach (var parameter in parameters)
                {
                    DbParameter dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = parameter.Key;
                    dbParameter.DbType = typeMap[parameter.Value.GetType()];
                    dbParameter.Value = parameter.Value;
                    command.Parameters.Add(dbParameter);
                }

                _context.Database.OpenConnection();
                using (var record = command.ExecuteReader())
                {
                    List<T> items = new List<T>();
                    T obj = default(T);
                    while (record.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(record[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, record[prop.Name], null);
                            }
                        }
                        items.Add(obj);
                    }
                    _context.Database.CloseConnection();
                    return items;
                }
            }
        }

        /// <summary>
        /// Map DbType from System.Type
        /// </summary>
        /// <returns></returns>
        private Dictionary<Type, DbType> GetTypes()
        {
            Dictionary<Type, DbType> typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;

            return typeMap;
        }

        #region Audit Log

       
        /// <summary>
        /// This method get a changes context
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public Dictionary<string, List<object>> GetChanged(ref EntityState state)
        {
            Dictionary<string, List<object>> keyValues = new Dictionary<string, List<object>>();
            IEnumerable<EntityEntry> changes = from e in _context.ChangeTracker.Entries().AsEnumerable()
                                               where e.State != EntityState.Unchanged
                                               select e;

            foreach (var change in changes)
            {
                var entity = change.Entity;
                state = change.State;
                Assembly assembly = Assembly.Load(entity.GetType().Assembly.FullName);
                Type tipo = assembly.GetType(entity.GetType().FullName);
                object _entity = Convert.ChangeType(entity, tipo);

                if (keyValues.ContainsKey(entity.GetType().Name))
                {
                    keyValues[entity.GetType().Name].Add(_entity);
                }
                else
                {
                    keyValues.Add(entity.GetType().Name, new List<object>() { _entity });
                }
            }

            return keyValues;
        }


        #endregion Audit Log
    }
}