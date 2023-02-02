// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.

using System;

// TODO: PREVENT CREATION OF A COPY

class Vehicle
{
    public static string GenerateRandomRegistrationPlateNumber()
    {
        // TODO: proper registration plate generator

        char[] letters = new char[4];

        int[] nums = new int[4];

        var rand = new Random();
        for (int i = 0; i < 4; i++)
        {
            letters[i] = (char)rand.Next(129);
            nums[i] = rand.Next(10);
        }

        return letters[0] + letters[1] + "-" + nums.ToString() + "-" + letters[1] + letters[1];
    }

    public Vehicle(string identifier, VehicleType vehicleType, decimal balance)
    {

    }

    public void ChangeBalance(decimal value)
    {
        Balance += value;
    }

    readonly public string Identifier;
    readonly public VehicleType VehicleType;

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
}