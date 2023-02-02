// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.

using System;
using System.Collections.Generic;

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
    private Parking() { }

    public decimal Balance
    {
        get
        {
            return _balance;
        }

        private set
        {
            _balance = value;
        }
    }

    private decimal _balance;

    private List<Vehicle> _vehiclesOnBalance;

    private static object _lock = new object();
    private static Parking _instance = null;
}