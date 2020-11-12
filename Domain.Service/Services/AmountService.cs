namespace Domain.Service.Services
{
    using Common.Utils.Constant;
    using Common.Utils.Enums;
    using Common.Utils.Excepcions;
    using Common.Utils.Resources;
    using Domain.Service.Services.Interface;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Infraestructure.Entity.Entities.General;
    using System;
    using static Common.Utils.Enums.Enums;

    public class AmountService : IAmountService
    {

        #region Members
        public readonly IUnitOfWork unitOfWork;
        #endregion Members

        #region Builder
        public AmountService(IUnitOfWork iUnitOfWork)
        {
            unitOfWork = iUnitOfWork;
        }
        #endregion Builder       

        /// <summary>
        /// Create Amount for a Vehicle Type
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="oVehicleType"></param>
        /// <returns></returns>
        public bool CreateAmount(decimal Amount, VehicleType oVehicleType)
        {
            AmountEntity oAmount = new AmountEntity()
            {
                CreationDate = DateTime.Now,
                CreationUser = UsersParkingLot.System,
                Amount = Amount,
                VehicleType = (int)oVehicleType
            };

            if (!Enum.IsDefined(typeof(VehicleType), oVehicleType))
                throw new BusinessExeption(GeneralMessages.VehicleTypeNotSupported);

            if (oAmount.VehicleType == (int)VehicleType.NotType)
                throw new BusinessExeption(string.Format(GeneralMessages.VehicleTypeValueAmountInvalid, VehicleType.NotType.GetDisplayName()));

            unitOfWork.AmountRepository.Insert(oAmount);

            return unitOfWork.Save() > 0;
        }

        /// <summary>
        /// Get Vigent Account for a Vehicle Type
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <returns></returns>
        public AmountEntity GetVigentAmount(VehicleType vehicleType)
        {
            return unitOfWork
               .AmountRepository
               .LastOrDefault(g => g.VehicleType.Equals((int)vehicleType));
        }

    }
}
