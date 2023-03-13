using AutoMapper;

namespace CoolParking.WebAPI.Models
{
    public static class VehicleAutoMapperConfiguration
    {
        public static MapperConfiguration GetVehicleToDTOConfiguration()
        {
            if(_vehicleToDTO == null)
            {
                _vehicleToDTO = new MapperConfiguration(cfg => cfg.CreateMap(typeof(Vehicle), typeof(VehicleDataTransfer)));
            }

            return _vehicleToDTO;
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
        //private static MapperConfiguration _DTOToVehicle = null;
    }
}
