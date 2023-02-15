// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.

using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

public static class Settings
{
    public static int ParkingCapacity = 10;
    public static ushort WithdrawalTimeIntervalInSeconds = 5; 

    public static string LogPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log";

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

    public static string VehicleIDTemplate = @"[A-Z]{2}[-][\d]{4}[-][A-Z]{2}";
}