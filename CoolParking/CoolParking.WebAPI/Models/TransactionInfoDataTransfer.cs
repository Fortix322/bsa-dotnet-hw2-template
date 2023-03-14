
namespace CoolParking.WebAPI.Models
{
    public class TransactionInfoDataTransfer
    {

        public override string ToString()
        {
            return $"{TransactionTime}: {Sum} money withdrawn from vehicle with Id='{VehicleId}'.\n";
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
}
