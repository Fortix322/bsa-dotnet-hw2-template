// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.

using System.IO;
using System.Reflection;

public static class Settings
{
    public static readonly int ParkingCapacity = 10;
    public static readonly decimal ParkingStartingBalance = 0;

    public static readonly ushort WithdrawalTimeIntervalInSeconds = 5;
    public static readonly ushort LoggingTimeIntervalInSeconds = 60;

    public static readonly string LogPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log";

    public static readonly string VehicleIdPattern = @"[A-Z]{2}[-]\d{4}[-][A-Z]{2}";

    public static readonly decimal PenaltyRate = 2.5M;

    public static decimal GetWithdrawalValueByVehicleType(VehicleType vehicleType)
    {
        switch(vehicleType)
        {
            case VehicleType.Motorcycle:
                return 1M;
            case VehicleType.Bus:
                return 3.5M;
            case VehicleType.PassengerCar:
                return 2M;
            case VehicleType.Truck:
                return 5M;
            default:
                return 0M;
        }
    }

}