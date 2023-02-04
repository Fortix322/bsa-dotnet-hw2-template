// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.

using CoolParking.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

internal class Parking
{
    public static Parking GetInstance()
    {
        lock(_lock)
        {
            if(_instance == null)
            {
                _instance = new Parking();
            }
        }

        return _instance;
    }

    private Parking() 
    {
        if(Settings.ParkingCapacity < 0)
        {
            throw new ArgumentException();
        }
        Capacity = Settings.ParkingCapacity;
    }

    public void SetTimer(ITimerService timer)
    {
        if(_timer != null)
        {
            _timer = timer;
            _timer.Elapsed += FireWithdraw;
            _timer.Start();

        }
    }

    public ReadOnlyDictionary<string, Vehicle> GetVehicles()
    {
        return new ReadOnlyDictionary<string, Vehicle>(_vehiclesOnBalance);
    }

    public bool AddVehicle(Vehicle vehicle)
    {
        if(_vehiclesOnBalance.Count + 1 < Capacity)
        {
            _vehiclesOnBalance.Add(vehicle.Identifier, vehicle);
            return true;
        }
        return false;
    }

    public void RemoveVehicle(string id)
    {
        _vehiclesOnBalance.Remove(id);
    }

    public event Withdraw OnWithdraw;
    public delegate void Withdraw(ReadOnlyCollection<Vehicle> vehiclesList);

    public readonly int Capacity;
    public decimal Balance
    {
        get
        {
            return _balance;
        }

        private set
        {
            if (value < 0) throw new ArgumentException();
            _balance = value;
        }
    }

    private decimal _balance;

    private void FireWithdraw(object sender, ElapsedEventArgs e)
    {
        OnWithdraw.Invoke(_vehiclesOnBalance.Values.ToList().AsReadOnly());
    }

    private ITimerService _timer;

    private Dictionary<string, Vehicle> _vehiclesOnBalance;

    private static object _lock = new object();
    private static Parking _instance = null;
}