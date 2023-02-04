// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.

static class Settings
{
    public static int ParkingCapacity = 50000;
    public static ushort WithdrawalTimeIntervalInSeconds = 5; 

    public static decimal WithdrawalValueByVehicleType(VehicleType vehicleType)
    {
        switch(vehicleType)
        {
            case VehicleType.Motorcycle:
                return 1;
            case VehicleType.Bus:
                return 3.5M;
            case VehicleType.PassengerCar:
                return 2;
            case VehicleType.Truck:
                return 5;
            default:
                return 0;
        }
    }
}