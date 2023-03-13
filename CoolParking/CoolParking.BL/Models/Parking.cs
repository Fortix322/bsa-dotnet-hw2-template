// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

    internal static void ResetInstance()
    {
        _instance._vehiclesOnBalance.Clear();
        _instance.Balance = Settings.ParkingStartingBalance;
    }

    private Parking() 
    {
        if(Settings.ParkingCapacity < 0)
        {
            throw new ArgumentException();
        }
        Capacity = Settings.ParkingCapacity;
        Balance = Settings.ParkingStartingBalance;
        _vehiclesOnBalance = new Dictionary<string, Vehicle>();
    }

    public ReadOnlyCollection<Vehicle> GetVehicles()  
    {
        return _vehiclesOnBalance.Values.ToList().AsReadOnly();
    }

    public bool ContainsVehicle(string Id, out Vehicle vehicle)
    {
        return _vehiclesOnBalance.TryGetValue(Id, out vehicle);
    }

    public bool AddVehicle(Vehicle vehicle)
    {
        if(_vehiclesOnBalance.Count + 1 <= Capacity)
        {
            try
            {
                _vehiclesOnBalance.Add(vehicle.Id, vehicle);
            }
            catch(Exception)
            {
                throw new ArgumentException();
            }

            return true;
        }

        return false;
    }

    public void RemoveVehicle(string Id)
    {
        _vehiclesOnBalance.Remove(Id);
    }

    public readonly int Capacity;
    public decimal Balance
    {
        get
        {
            return _balance;
        }

        internal set
        {
            if (value < 0) throw new ArgumentException();
            _balance = value;
        }
    }

    private decimal _balance;

    private Dictionary<string, Vehicle> _vehiclesOnBalance;

    private static object _lock = new object();
    private static Parking _instance = null;
}