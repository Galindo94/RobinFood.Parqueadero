namespace Domain.Service.Services
{
    using Common.Utils.Constant;
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

        public bool CreateAmount(decimal Amount, VehicleType VehicleType)
        {
            AmountEntity oAmount = new AmountEntity()
            {
                CreationDate = DateTime.Now,
                CreationUser = UsersParkingLot.System,
                Amount = Amount,
                VehicleType = (int)VehicleType
            };

            unitOfWork.AmountRepository.Insert(oAmount);

            return unitOfWork.Save() > 0;
        }

        public AmountEntity GetVigentAmount(VehicleType vehicleType)
        {
            return unitOfWork
               .AmountRepository
               .LastOrDefault(g => g.VehicleType.Equals((int)vehicleType));
        }

    }
}
