using System.Text.RegularExpressions;

namespace CoolParking.WebAPI.Models
{
    public static class RegistrationPlate
    {
        public static bool IsPlateNumberMatchesPattern(string plateNumber)
        {
            Regex pattern = new Regex(Settings.VehicleIdPattern);
            return pattern.Matches(plateNumber).Count == 1;
        }
    }
}
