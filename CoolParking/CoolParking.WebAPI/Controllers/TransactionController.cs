using AutoMapper;
using CoolParking.BL.Interfaces;
using CoolParking.WebAPI.Models;
using CoolParking.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly IParkingService _parkingService;

        public TransactionController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpGet("last")]
        public IActionResult GetLastTransaction()
        {
            var transactions = _parkingService.GetLastParkingTransactions();

            TransactionInfoDataTransfer transactionDTO = null;

            if(transactions.Length != 0)
            {
                var mapper = new Mapper(AutoMapperConfiguration.GetTransactionToDTOConfiguration());

                transactionDTO = mapper.Map<TransactionInfo, TransactionInfoDataTransfer>(transactions.LastOrDefault());
            }

            return Ok(transactionDTO);
        }

        [HttpGet("all")]
        public IActionResult GetTransactionsFromLog()
        {
            string logInfo;

            try
            {
                logInfo = _parkingService.ReadFromLog();
            }
            catch(Exception)
            {
                return NotFound();
            }

            return Ok(logInfo);
        }

        [HttpPut("topUpVehicle")]
        public IActionResult TopUpVehicle(TopUpVehicleDataTransfer topUpVehicleDataTransfer)
        {
            if (topUpVehicleDataTransfer == null || !RegistrationPlate.IsPlateNumberMatchesPattern(topUpVehicleDataTransfer.ID)) return BadRequest();

            if (topUpVehicleDataTransfer.Sum < 0) return BadRequest();

            try
            {
                _parkingService.TopUpVehicle(topUpVehicleDataTransfer.ID, topUpVehicleDataTransfer.Sum);
            }
            catch (Exception)
            {
                return NotFound();
            }

            Vehicle vehicle;
            VehicleService.TryGetVehicleByID(_parkingService, topUpVehicleDataTransfer.ID, out vehicle);

            var mapper = new Mapper(AutoMapperConfiguration.GetVehicleToDTOConfiguration());

            var vehicleDTO = mapper.Map<Vehicle, VehicleDataTransfer>(vehicle);

            return Ok(vehicleDTO);
        }
    }
}
