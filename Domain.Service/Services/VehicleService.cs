namespace Domain.Service.Services
{
    using AutoMapper;
    using Common.Utils.Constant;
    using Common.Utils.Enums;
    using Common.Utils.Excepcions;
    using Common.Utils.Resources;
    using Domain.Service.Class;
    using Domain.Service.DTO.Vehicle;
    using Domain.Service.Services.Abstract;
    using Domain.Service.Services.Interface;
    using Infraestructure.Core.UnitOfWork.Interface;
    using Infraestructure.Entity.Entities.InputsOutputs;
    using Infraestructure.Entity.Entities.Vehicle;
    using System;
    using static Common.Utils.Enums.Enums;

    public class VehicleService : IVehicleService
    {

        #region Members

        public readonly IUnitOfWork unitOfWork;

        #endregion Members

        #region Builder

        public VehicleService(IUnitOfWork iUnitOfWork)
        {
            unitOfWork = iUnitOfWork;
        }

        #endregion Builder       

        public CreateVehicleResponseDto CreateVehicle(string VehiclePlate, VehicleType VehicleType)
        {
            CreateVehicleResponseDto oResponse = new CreateVehicleResponseDto();

            if (!ValidateExistVehicle(VehiclePlate))
            {
                if (VehicleType != VehicleType.NotType && VehicleType != VehicleType.NotResident)
                {
                    VehicleDto oVehicleDto = new VehicleDto
                    {
                        VehiclePlate = VehiclePlate,
                        VehicleType = (int)VehicleType,
                    };

                    VehicleEntity oVehicle = Mapper.Map<VehicleEntity>(oVehicleDto);
                    oVehicle.CreationDate = DateTime.Now;
                    oVehicle.CreationUser = UsersParkingLot.System;
                    oVehicle.CutoffDate = DateTime.Now;

                    unitOfWork.VehicleRepository.Insert(oVehicle);

                    if (unitOfWork.Save() > 0)
                    {
                        oResponse.Inserted = true;
                        oResponse.Message = GeneralMessages.ItemInserted;
                    }
                }
                else
                    oResponse.Message = string.Format(GeneralMessages.VehicleTypeRegistrationNotAllowed, VehicleType.NotType.GetDisplayName(), VehicleType.NotResident.GetDisplayName());
            }
            else
                oResponse.Message = string.Format(GeneralMessages.ExistVehicle, VehiclePlate.ToUpper());

            return oResponse;

        }

        public CreateVehicleResponseDto RegisterInput(string VehiclePlate)
        {
            CreateVehicleResponseDto oResponse = new CreateVehicleResponseDto();

            ControlInputsOutputsEntity oInput = new ControlInputsOutputsEntity
            {
                CreationDate = DateTime.Now,
                CreationUser = UsersParkingLot.System,
                VehiclePlate = VehiclePlate,
                EntryTime = DateTime.Now,
                TotalMinutesOfStay = 0
            };

            if (!ValidateInputNotOpen(VehiclePlate))
            {
                VehicleEntity oVehicle = GetVehicle(VehiclePlate);

                if (oVehicle != null)
                    oInput.VehicleType = oVehicle.VehicleType;
                else
                    oInput.VehicleType = (int)VehicleType.NotResident;

                unitOfWork.ControlInputsOutputsRepository.Insert(oInput);

                if (unitOfWork.Save() > 0)
                {
                    oResponse.Inserted = true;
                    oResponse.Message = GeneralMessages.ItemInserted;
                }
            }
            else
                oResponse.Message = string.Format(GeneralMessages.OpenCloseEntrance, TypeMovement.Input, VehiclePlate.ToUpper(), ExistMovement.Already);

            return oResponse;
        }

        public RegisterOutPutResponseDto RegisterOutPut(string VehiclePlate)
        {
            RegisterOutPutResponseDto oResponse = new RegisterOutPutResponseDto();
            AVehicleBase aVehicle;

            if (ValidateInputNotOpen(VehiclePlate))
            {
                VehicleEntity oVehicle = GetVehicle(VehiclePlate);

                if (oVehicle != null)
                    aVehicle = GetVehicleBase(oVehicle, (VehicleType)Enum.ToObject(typeof(VehicleType), oVehicle.VehicleType));
                else
                {
                    oVehicle = new VehicleEntity
                    {
                        VehicleType = (int)VehicleType.NotResident
                    };

                    aVehicle = GetVehicleBase(oVehicle, VehicleType.NotResident);
                }

                if (aVehicle != null)
                {
                    ControlInputsOutputsEntity oControlInputsOutputs = unitOfWork
                        .ControlInputsOutputsRepository
                        .FirstOrDefault(g => g.VehiclePlate.ToUpper().Equals(VehiclePlate));

                    Decimal oAmountToBePaid = aVehicle.RegisterOutPut(oControlInputsOutputs);

                    if (unitOfWork.Save() > 0)
                    {
                        oResponse.Inserted = true;
                        oResponse.Message = GeneralMessages.ItemInserted;
                        oResponse.AmountToBePaid = oAmountToBePaid;
                    }
                }
                else
                    throw new BusinessExeption(string.Format(GeneralMessages.VehicleAndTypeVehicleNotFound, VehiclePlate, oVehicle.VehicleType));
            }
            else
                oResponse.Message = string.Format(GeneralMessages.OpenCloseEntrance, TypeMovement.OutPut, VehiclePlate.ToUpper(), ExistMovement.No);

            return oResponse;
        }

        public VehicleEntity GetVehicle(string VehiclePlate)
        {
            return unitOfWork
                .VehicleRepository
                .FirstOrDefault(g => g.VehiclePlate.ToUpper().Equals(VehiclePlate.ToUpper()),
                                g => g.PaymentVehicleEntities);
        }

        private AVehicleBase GetVehicleBase(VehicleEntity oVehicle, VehicleType vehicleType)
        {
            AVehicleBase aVehicle;

            switch (vehicleType)
            {
                case VehicleType.NotType:
                    aVehicle = null;
                    break;

                case VehicleType.VIP:
                    aVehicle = new VIPVehicle(oVehicle, unitOfWork);
                    break;
                case VehicleType.Resident:
                    aVehicle = new ResidentVehicle(oVehicle, unitOfWork);
                    break;
                case VehicleType.PostPaidResident:
                    aVehicle = new PostPaidResidentVehicle(oVehicle, unitOfWork);
                    break;
                case VehicleType.NotResident:
                    aVehicle = new NoResidentVehicle(oVehicle, unitOfWork);
                    break;

                default:
                    aVehicle = null;
                    break;
            }

            return aVehicle;
        }

        private bool ValidateExistVehicle(string VehiclePlate)
        {
            return unitOfWork.VehicleRepository.FirstOrDefault(g => g.VehiclePlate.ToUpper().Equals(VehiclePlate.ToUpper())) != null;
        }

        private bool ValidateInputNotOpen(string VehiclePlate)
        {
            return unitOfWork.ControlInputsOutputsRepository.FirstOrDefault(g => g.VehiclePlate.ToUpper().Equals(VehiclePlate.ToUpper()) && g.DepartureTime == null) != null;
        }
    }
}
