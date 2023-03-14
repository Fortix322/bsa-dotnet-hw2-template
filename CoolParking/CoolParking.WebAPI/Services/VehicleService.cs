using CoolParking.BL.Interfaces;

namespace CoolParking.WebAPI.Services
{
    public class VehicleService
    {
        public static bool TryGetVehicleByID(IParkingService parkingService, string id, out Vehicle vehicle)
        {
            vehicle = null;

            if(parkingService is ParkingService)
            {
                return ((ParkingService)parkingService).TryGetVehicleByID(id, out vehicle);
            }

            var vehicles = parkingService.GetVehicles();

            foreach (Vehicle v in vehicles)
            {
                if (v.Id == id) vehicle = v;
            }

            if (vehicle != null) return true;
            return false;
        }
    }
}
