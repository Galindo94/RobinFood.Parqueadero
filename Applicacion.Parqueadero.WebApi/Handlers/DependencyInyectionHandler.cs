namespace Applicacion.Transferencias.WebApi.Handlers
{
    using Common.Utils.Utils;
    using Common.Utils.Utils.Interface;
    using Domain.Service.Services;
    using Domain.Service.Services.Interface;
    using Infraestructure.Core.Repository;
    using Infraestructure.Core.Repository.Interface;
    using Infraestructure.Core.UnitOfWork;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using System.Diagnostics.CodeAnalysis;

    public static class DependencyInyectionHandler
    {
        [ExcludeFromCodeCoverage]
        public static void DependencyInyectionConfig(IServiceCollection services)
        {
            #region Register (dependency injection)

            // Repository await UnitofWork parameter ctor explicit
            services.AddScoped<UnitOfWork, UnitOfWork>();

            // Infrastructure
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            // Domain
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IAmountService, AmountService>();
            services.AddTransient<IResidentPaymentsService, ResidentPaymentsService>();

            // Common            

            //Utils
            services.AddTransient<IUtils, Utils>();


            #endregion Register (dependency injection)
        }
    }
}