// TODO: implement struct TransactionInfo.
//       Necessarily implement the Sum property (decimal) - is used in tests.
//       Other implementation details are up to you, they just have to meet the requirements of the homework.

using System;
using System.Transactions;

public struct TransactionInfo
{
    public TransactionInfo(string vehicleId, decimal sum)
    {
        TransactionTime = DateTime.Now;
        VehicleId = vehicleId;
        Sum = sum;
    }

    public override string ToString()
    {
        return $"[{TransactionTime}] {VehicleId} {Sum}";
    }

    public decimal Sum
    {
        get; set;
    }
    public string VehicleId
    {
        get; set;
    }
    public DateTime TransactionTime
    {
        get; set;
    }
}