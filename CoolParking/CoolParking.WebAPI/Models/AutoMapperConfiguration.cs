using AutoMapper;

namespace CoolParking.WebAPI.Models
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration GetVehicleToDTOConfiguration()
        {
            if(_vehicleToDTO == null)
            {
                _vehicleToDTO = new MapperConfiguration(cfg => cfg.CreateMap(typeof(Vehicle), typeof(VehicleDataTransfer)));
            }

            return _vehicleToDTO;
        }

        public static MapperConfiguration GetTransactionToDTOConfiguration()
        {
            if (_transactionToDTO == null)
            {
                _transactionToDTO = new MapperConfiguration(cfg => cfg.CreateMap(typeof(TransactionInfo), typeof(TransactionInfoDataTransfer)));
            }

            return _transactionToDTO;
        }
        //public static MapperConfiguration GetDTOToVehicleConfiguration()
        //{
        //    if (_vehicleToDTO == null)
        //    {
        //        _vehicleToDTO = new MapperConfiguration(cfg => cfg.CreateMap(typeof(Vehicle), typeof(VehicleDataTransfer)));
        //    }

        //    return _vehicleToDTO;
        //}

        private static MapperConfiguration _vehicleToDTO = null;
        private static MapperConfiguration _transactionToDTO = null;
        //private static MapperConfiguration _DTOToVehicle = null;
    }
}
