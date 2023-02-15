// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.

using CoolParking.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Timers;

public class ParkingService : IParkingService
{
    public ParkingService(ITimerService withdrawalTimer, ITimerService logTimer, ILogService logService)
    {
        _parking = Parking.GetInstance();

        _transactionsLog = logService;

        _withdrawalTimer = withdrawalTimer;
        _transactionLoggingTimer = logTimer;

        _transactionLoggingTimer.Elapsed += OnTimerElapsed;
        _transactionLoggingTimer.Start();
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (!_parking.AddVehicle(vehicle))
            throw new InvalidOperationException();
    }

    public void Dispose() {}

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
        throw new NotImplementedException();
    }

    public ReadOnlyCollection<Vehicle> GetVehicles()
    {
        return _parking.GetVehicles();
    }

    public string ReadFromLog()
    {
        throw new NotImplementedException();
    }

    public void RemoveVehicle(string vehicleId)
    {
        Vehicle vehicle;

        if(_parking.ContainsVehicle(vehicleId, out vehicle);)
        {
            if (vehicle.Balance < 0) throw new InvalidOperationException();
            _parking.RemoveVehicle(vehicleId);
        }
    }

    public void TopUpVehicle(string vehicleId, decimal sum)
    {
        Vehicle vehicle;

        if(_parking.ContainsVehicle(vehicleId, out vehicle))
            vehicle.ChangeBalance(sum);
    }

    private void MakeWithdraw(ReadOnlyCollection<Vehicle> vehicles)
    {
        foreach(Vehicle vehicle in vehicles)
        {
            vehicle.ChangeBalance(-Settings.WithdrawalValueByVehicleType(vehicle.VehicleType));
        }
    }

    private void WriteTransactionsToLog()
    { 
        foreach(TransactionInfo transaction in _transactions)
        {
            string logInfo  = "[" + transaction.TransactionTime + "] " + transaction.VehicleId + " " + transaction.Sum + '\n';
            _transactionsLog.Write(logInfo);
        }
        _transactions.Clear();
    }

    void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if(sender == _transactionLoggingTimer)
        {
            WriteTransactionsToLog();
        }
    }

    private Parking _parking;

    private List<TransactionInfo> _transactions;

    private ILogService _transactionsLog;

    private ITimerService _transactionLoggingTimer;
    private ITimerService _withdrawalTimer;
}