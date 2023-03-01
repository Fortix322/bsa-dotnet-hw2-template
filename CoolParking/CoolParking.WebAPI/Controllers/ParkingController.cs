using Microsoft.AspNetCore.Mvc;
using CoolParking.BL.Interfaces;
using System.Text.Json;

namespace CoolParking.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController : Controller
    {
        private readonly IParkingService _parkingService;

        public ParkingController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpGet("freePlaces")]
        public IActionResult GetFreePlaces()
        {
            return Ok(_parkingService.GetFreePlaces());
        }

        [HttpGet("balance")]
        public IActionResult GetBalance()
        {
            return Ok(_parkingService.GetBalance());
        }

        [HttpGet("capacity")]
        public IActionResult GetCapacity()
        {
            return Ok(_parkingService.GetCapacity());
        }
    }
}
