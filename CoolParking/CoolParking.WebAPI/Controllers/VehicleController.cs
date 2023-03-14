using AutoMapper;
using CoolParking.BL.Interfaces;
using CoolParking.WebAPI.Models;
using CoolParking.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CoolParking.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : Controller
    {
        private readonly IParkingService _parkingService;

        public VehicleController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpGet()]
        public IActionResult GetVehicles()
        {
            var vehicles = _parkingService.GetVehicles();
            if (vehicles.Count == 0) return Ok(vehicles);

            var mapper = new Mapper(AutoMapperConfiguration.GetVehicleToDTOConfiguration());

            var vehiclesDTO = mapper.Map<IList<Vehicle>, List<VehicleDataTransfer>>(vehicles);

            return Ok(vehiclesDTO);
        }

        [HttpPost()]
        public IActionResult PostVehicle([FromBody] VehicleDataTransfer vehicle)
        {
            if (vehicle == null || !RegistrationPlate.IsPlateNumberMatchesPattern(vehicle.ID)) return BadRequest();

            if (vehicle.Balance < 0 || !Enum.IsDefined(vehicle.VehicleType)) return BadRequest();

            try
            {
                _parkingService.AddVehicle(new Vehicle(vehicle.ID, vehicle.VehicleType, vehicle.Balance));
            }
            catch (Exception)
            {
                return BadRequest();
            }

            var requestUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            return Created(requestUrl, vehicle);
        }


        [HttpGet("{id}")]
        public IActionResult GetVehicleByID([FromRoute]string id)
        {
            if (!RegistrationPlate.IsPlateNumberMatchesPattern(id)) return BadRequest();

            Vehicle vehicle;
            if (!VehicleService.TryGetVehicleByID(_parkingService, id, out vehicle)) return NotFound();

            var mapper = new Mapper(AutoMapperConfiguration.GetVehicleToDTOConfiguration());

            var vehicleDTO = mapper.Map<Vehicle, VehicleDataTransfer>(vehicle);

            return Ok(vehicleDTO);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVehicle([FromRoute] string id)
        {
            if (!RegistrationPlate.IsPlateNumberMatchesPattern(id)) return BadRequest();

            Vehicle vehicle;
            if (!VehicleService.TryGetVehicleByID(_parkingService, id, out vehicle)) return NotFound();

            try
            {
                _parkingService.RemoveVehicle(id);
            }
            catch(Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
