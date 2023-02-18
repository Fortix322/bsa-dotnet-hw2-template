// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.

using CoolParking.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Timers;

public class ParkingService : IParkingService
{
    public ParkingService(ITimerService withdrawalTimer, ITimerService logTimer, ILogService logService)
    {
        _parking = Parking.GetInstance();

        _transactions = new List<TransactionInfo>();

        _transactionsLog = logService;

        _withdrawalTimer = withdrawalTimer;
        _transactionLoggingTimer = logTimer;

        _transactionLoggingTimer.Elapsed += OnTimerElapsed;
        _transactionLoggingTimer.Start();

        _withdrawalTimer.Elapsed += OnTimerElapsed;
        _withdrawalTimer.Start();
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (!_parking.AddVehicle(vehicle))
            throw new InvalidOperationException();
    }

    public void Dispose() 
    {
        _withdrawalTimer.Dispose();
        _transactionLoggingTimer.Dispose();
        Parking.ResetInstance();
    }

    public decimal GetBalance()
    {
        return _parking.Balance;
    }

    public int GetCapacity()
    {
        return _parking.Capacity;
    }

    public int GetFreePlaces()
    {
        return _parking.Capacity - _parking.GetVehicles().Count;
    }

    public TransactionInfo[] GetLastParkingTransactions()
    {
        return _transactions.ToArray();
    }

    public ReadOnlyCollection<Vehicle> GetVehicles()
    {
        return _parking.GetVehicles();
    }

    public string ReadFromLog()
    {
        try
        {
            return _transactionsLog.Read();
        }
        catch(FileNotFoundException)
        {
            throw new InvalidOperationException();
        }
    }

    public void RemoveVehicle(string vehicleId)
    {
        Vehicle vehicle;

        if (!_parking.ContainsVehicle(vehicleId, out vehicle))
            throw new ArgumentException();

        if (vehicle.Balance < 0) throw new InvalidOperationException();
        _parking.RemoveVehicle(vehicleId);
        
    }

    public void TopUpVehicle(string vehicleId, decimal sum)
    {
        Vehicle vehicle;

        if (sum < 0) throw new ArgumentException();

        if (!_parking.ContainsVehicle(vehicleId, out vehicle))
            throw new ArgumentException();

        vehicle.Balance += sum;
    }

    private void MakeWithdraw(ReadOnlyCollection<Vehicle> vehicles)
    {
        foreach(Vehicle vehicle in vehicles)
        {
            decimal sum = Settings.GetWithdrawalValueByVehicleType(vehicle.VehicleType);
            if (sum > vehicle.Balance) sum = Math.Abs(vehicle.Balance - sum) + (sum - vehicle.Balance) * Settings.PenaltyRate;
            vehicle.Balance -= sum;
            _parking.Balance += sum;
            _transactions.Add(new TransactionInfo(vehicle.Id, sum));
        }
    }

    private void WriteTransactionsToLog()
    {
        StringBuilder stringBuilder = new StringBuilder(default(string));
        foreach(TransactionInfo transaction in _transactions)
        {
            stringBuilder.AppendLine($"[{transaction.TransactionTime}] {transaction.VehicleId} {transaction.Sum}");
        }
        _transactionsLog.Write(stringBuilder.ToString());
        _transactions.Clear();
    }

    void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        if(sender == _transactionLoggingTimer)
        {
            WriteTransactionsToLog();
        }
        else if(sender == _withdrawalTimer)
        {
            MakeWithdraw(GetVehicles());
        }
    }

    private Parking _parking;

    private List<TransactionInfo> _transactions;

    private ILogService _transactionsLog;

    private ITimerService _transactionLoggingTimer;
    private ITimerService _withdrawalTimer;
}