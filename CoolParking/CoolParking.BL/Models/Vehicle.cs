// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using Fare;

public class Vehicle
{
    public static string GenerateRandomRegistrationPlateNumber()
    {
        Xeger xeger = new Xeger(Settings.VehicleIdPattern);
        return xeger.Generate();
    }

    public Vehicle(string identifier, VehicleType vehicleType, decimal balance)
    {
        Regex vehicleNumberPattern = new Regex(Settings.VehicleIdPattern);

        if (balance < 0 || vehicleNumberPattern.Matches(identifier).Count != 1) throw new ArgumentException();

        Id = identifier;
        VehicleType = vehicleType;
        Balance = balance;
    }

    readonly public string Id;
    readonly public VehicleType VehicleType;

    public decimal Balance
    {
        get
        {
            return _balance;
        }

        internal set
        {
            _balance = value;
        }
    }

    private decimal _balance;
}