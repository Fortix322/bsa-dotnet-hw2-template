// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.

using CoolParking.BL.Interfaces;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

public class ParkingService : IParkingService
{

    public ParkingService(ITimerService withdrawalTimer, ITimerService logTimer)
    {
        _parking = Parking.GetInstance();
        _parking.OnWithdraw += MakeWithdraw;
        _parking.SetTimer(withdrawalTimer);
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (!_parking.AddVehicle(vehicle))
            throw new InvalidOperationException();
    }

    public void Dispose()
    {
        _parking.OnWithdraw -= MakeWithdraw;
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
        throw new NotImplementedException();
    }

    public ReadOnlyCollection<Vehicle> GetVehicles()
    {
       return new ReadOnlyCollection<Vehicle>(_parking.GetVehicles().Values.ToImmutableList()); // ???
    }

    public string ReadFromLog()
    {
        throw new NotImplementedException();
    }

    public void RemoveVehicle(string vehicleId)
    {
        Vehicle vehicle = _parking.GetVehicles()[vehicleId];
        if(vehicle != null)
        {
            if (vehicle.Balance < 0) throw new InvalidOperationException();
            _parking.RemoveVehicle(vehicleId);
        }
    }

    public void TopUpVehicle(string vehicleId, decimal sum)
    {
        _parking.GetVehicles()[vehicleId].ChangeBalance(sum);
    }

    private void MakeWithdraw(ReadOnlyCollection<Vehicle> vehicles)
    {
        foreach(Vehicle vehicle in vehicles)
        {
            vehicle.ChangeBalance(-Settings.WithdrawalValueByVehicleType(vehicle.VehicleType));
        }
    }

    private Parking _parking;
}