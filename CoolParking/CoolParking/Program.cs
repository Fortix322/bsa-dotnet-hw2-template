using Sharprompt;

class CoolParking
{
    public static void Main()
    {
        ParkingService parkingService = new ParkingService(new TimerService(Settings.WithdrawalTimeIntervalInSeconds), new TimerService(Settings.LoggingTimeIntervalInSeconds), new LogService(Settings.LogPath));

        bool shutdown = false;

        while(!shutdown)
        {
            PrintListOfCommand();
            int command = Prompt.Input<int>("Enter command");
            switch (command)
            {
                case 0:
                    shutdown = true;
                    break;
                case 1:
                    try
                    {
                        PutVehicleInParking(parkingService);
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Parking hasn`t available places");
                    }
                    break;
                case 2:
                    try
                    {
                        GetVehicleOutFromParking(parkingService);
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Vehicle doesn`t exist or has a negative balance");
                    }
                    break;
                case 3:
                    TopUpVehicleBalance(parkingService);
                    break;
                case 4:
                    GetFreeSpaceInParking(parkingService);
                    break;
                case 5:
                    GetBalanceOfParking(parkingService);
                    break;
                case 6:
                    PrintAllVehicles(parkingService);
                    break;
                case 7:
                    PrintLastEarnedProfit(parkingService);
                    break;
                case 8:
                    PrintTransactions(parkingService.GetLastParkingTransactions());
                    break;
                case 9:
                    string logData = parkingService.ReadFromLog();
                    if (logData == default) Console.WriteLine("Log is empty");
                    Console.Write(logData);
                    break;
                case 10:
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Wrong command. Try again");
                    break;
            }

        }

    }

    private static void PrintListOfCommand() => Console.WriteLine(
                                                "0 - Exit\n" +
                                                "1 - Put vehicle in parking\n" +
                                                "2 - Get vehicle out from parking\n" +
                                                "3 - Top up balance of vehicle\n" +
                                                "4 - Get free space available in parking\n" +
                                                "5 - Get balance of parking\n" +
                                                "6 - Get all vehicles stored in parking\n" +
                                                "7 - Profit earned during last period\n" +
                                                "8 - Transaction created during last period\n" +
                                                "9 - Read log\n" +
                                                "10 - Clear console");

    private static void PutVehicleInParking(ParkingService parkingService)
    {
        string vehicleID;
        VehicleType vehicleType;
        decimal vehicleBalance;

        bool generateRandomVehicleID = Prompt.Confirm("Generate random vehicle plate number?");

        if (generateRandomVehicleID) vehicleID = Vehicle.GenerateRandomRegistrationPlateNumber();
        else
        {
            vehicleID = Prompt.Input<string>("Enter vehicle plate number(pattern: 'AA-1111-AA')", validators: new[] { Validators.RegularExpression(Settings.VehicleIdPattern) });
        }

        Console.WriteLine($"Vehicle ID: {vehicleID}");

        vehicleType = Prompt.Select<VehicleType>("Choose type of vehicle");

        vehicleBalance = Decimal.Parse(Prompt.Input<string>("Enter vehicle balance", validators: new[] { Validators.RegularExpression("^[+]?([.]\\d+|\\d+([.]\\d+)?)$") }));

        parkingService.AddVehicle(new Vehicle(vehicleID, vehicleType, vehicleBalance));
    }

    private static void GetVehicleOutFromParking(ParkingService parkingService)
    {
        IEnumerable<string> vehiclesID = AllVehicleIDIntoList(parkingService);

        if (vehiclesID.Count() == 0) Console.WriteLine("Parking hasn`t vehicle in it yet");
        else
        {
            var vehicleID = Prompt.Select<string>("Choose vehicle", AllVehicleIDIntoList(parkingService));
            parkingService.RemoveVehicle(vehicleID);
        }
    }

    private static void TopUpVehicleBalance(ParkingService parkingService)
    {
        IEnumerable<string> vehiclesID = AllVehicleIDIntoList(parkingService);

        if (vehiclesID.Count() == 0) Console.WriteLine("Parking hasn`t vehicle in it yet");
        else
        {
            var vehicleID = Prompt.Select<string>("Choose vehicle", AllVehicleIDIntoList(parkingService));
            decimal sumToTopUp = Decimal.Parse(Prompt.Input<string>("Enter vehicle balance", 
                                                validators: new[] { Validators.RegularExpression("^[+]?([.]\\d+|\\d+([.]\\d+)?)$") }));

            parkingService.TopUpVehicle(vehicleID, sumToTopUp);
        }
    }

    private static void GetFreeSpaceInParking(ParkingService parkingService)
    {
        Console.WriteLine("Parking has {0} free spaces", parkingService.GetFreePlaces());
    }

    private static void GetBalanceOfParking(ParkingService parkingService)
    {
        Console.WriteLine("Parking balance: {0}", parkingService.GetBalance());
    }

    private static IReadOnlyList<string> AllVehicleIDIntoList(ParkingService parkingService)
    {
        var vehicles = parkingService.GetVehicles();

        List<string> vehiclesID = new List<string>();

        foreach (Vehicle vehicle in vehicles)
        {
            vehiclesID.Add(vehicle.Id);
        }

        return vehiclesID.AsReadOnly();
    }

    private static void PrintAllVehicles(ParkingService parkingService)
    {
        var vehicles = parkingService.GetVehicles();

        if (vehicles.Count == 0) Console.WriteLine("Parking hasn`t any vehicles in it yet");

        foreach (Vehicle vehicle in vehicles)
        {
            Console.WriteLine($"Vehicle id: {vehicle.Id}");
            Console.WriteLine($"Vehicle type: {vehicle.VehicleType.ToString()}");
            Console.WriteLine($"Vehicle balance: {vehicle.Balance}\n");
        }
    }

    private static void PrintLastEarnedProfit(ParkingService parkingService)
    {
        var transactions = parkingService.GetLastParkingTransactions();

        decimal profit = 0M;

        foreach(TransactionInfo transaction in transactions)
        {
            profit += transaction.Sum;
        }

        Console.WriteLine($"Last earned profit: {profit}");
    }

    private static void PrintTransactions(TransactionInfo[] transactions)
    {
        if (transactions.Length == 0) Console.WriteLine("There is no transactions");

        foreach(TransactionInfo transaction in transactions)
        {
            Console.WriteLine("[{0}] {1} {2}", transaction.TransactionTime, transaction.VehicleId, transaction.Sum);
        }
    }
}
