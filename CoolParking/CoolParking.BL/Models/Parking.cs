// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.

using CoolParking.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;

class Parking
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
        _maxCapacity = Settings.MaxParkingCapacity;
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

    public ReadOnlyCollection<Vehicle> GetVehicles()
    {
        return _vehiclesOnBalance.AsReadOnly();
    }

    public bool AddVehicle(Vehicle vehicle)
    {
        if(_vehiclesOnBalance.Count + 1 < _maxCapacity)
        {
            _vehiclesOnBalance.Add(vehicle);
            return true;
        }

        return false;
    }

    public void RemoveVehicle(string id)
    {
        foreach(Vehicle v in _vehiclesOnBalance)
        {
            if (v.Identifier == id) _vehiclesOnBalance.Remove(v);
        }
    }

    public void RemoveVehicle(Vehicle vehicle)
    {
        _vehiclesOnBalance.Remove(vehicle);
    }

    public event Withdraw OnWithdraw;
    public delegate void Withdraw(ReadOnlyCollection<Vehicle> vehiclesList);

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
        OnWithdraw.Invoke(_vehiclesOnBalance.AsReadOnly());
    }

    private List<Vehicle> _vehiclesOnBalance;
    private ITimerService _timer;

    private readonly uint _maxCapacity;

    private static object _lock = new object();
    private static Parking _instance = null;
}