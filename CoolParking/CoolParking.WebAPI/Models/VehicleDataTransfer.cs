namespace CoolParking.WebAPI.Models
{
    public class VehicleDataTransfer
    {
        public string ID
        {
            get; set;
        }
        public VehicleType VehicleType
        {
            get; set;
        }
        public decimal Balance
        {
            get; set;
        }
    }
}
