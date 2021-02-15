using Microsoft.AspNetCore.Mvc;
using RBC_GAM.ModelDTO;
using RBC_GAM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RBC_GAM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialInstrumentController : ControllerBase
    {
        private readonly IFinancialInstrumentRepository _financialInstrumentRepository;

        public FinancialInstrumentController(IFinancialInstrumentRepository financialInstrumentRepository)
        {
            _financialInstrumentRepository = financialInstrumentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _financialInstrumentRepository.GetFinancialInstruments();

                return Ok(result);
            }
            catch
            {
                return BadRequest("Could not get Financial Instruments");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _financialInstrumentRepository.GetFinancialInstrument(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound("Could not find Financial Instrument");
        }

        [HttpPost("NewInstrument")]
        public async Task<IActionResult> NewInstrument([FromBody] FinInstrumentDTO finInst)
        {
            var result = await _financialInstrumentRepository.NewFinancialInstrument(finInst);
            if (result > 0)
                return Ok(result);
            else
                return BadRequest("Could not add new Financial Instrument");
        }

        [HttpPost("BuyInstrument")]
        public async Task<IActionResult> BuyInstrument([FromBody] UserDTO user)
        {
            var result = await _financialInstrumentRepository.BuyFinancialInstrument(user);
            if (result)
                return Ok();
            else
                return BadRequest("Could not buy new Financial Instrument");
        }

        [HttpPost("SellInstrument")]
        public async Task<IActionResult> SellInstrument([FromBody] UserDTO user)
        {
            var result = await _financialInstrumentRepository.SellFinancialInstrument(user);
            if (result)
                return Ok();
            else
                return BadRequest("Could not sell Financial Instrument");
        }

        [HttpPut("SetPrice")]
        public async Task<IActionResult> SetPrice([FromBody] FinInstrumentDTO finInst)
        {
            var result = await _financialInstrumentRepository.UpdatePrice(finInst);
            if (result)
                return Ok();
            else
                return BadRequest("Could not update price for Financial Instrument");
        }
    }
}
