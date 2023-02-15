using System.ComponentModel;

class CoolParking
{

    public static void Main()
    {
        ParkingService test = new ParkingService(new TimerService(5.0f), new TimerService(10.0f), new LogService(Settings.LogPath));

        Vehicle test1 = new Vehicle("test", VehicleType.Motorcycle, 10);
        Console.WriteLine(test.GetCapacity());

        test.AddVehicle(test1);

        Console.WriteLine(test.GetFreePlaces());

        Thread.Sleep(11000);
    }

}
