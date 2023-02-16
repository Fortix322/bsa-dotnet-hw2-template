// TODO: implement struct TransactionInfo.
//       Necessarily implement the Sum property (decimal) - is used in tests.
//       Other implementation details are up to you, they just have to meet the requirements of the homework.

using System;

public struct TransactionInfo
{
    public TransactionInfo(string vehicleId, decimal sum)
    {
        TransactionTime = DateTime.Now;
        VehicleId = vehicleId;
        Sum = sum;
    }

    public readonly decimal Sum;
    public readonly string VehicleId;
    public readonly DateTime TransactionTime;
}