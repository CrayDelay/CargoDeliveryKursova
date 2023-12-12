using Drivers.BLL.DTOs.Responses;
using Drivers.BLL.Managers;
using Drivers.BLL.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Drivers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly ILogger<TruckController> _logger;
        private ITruckManager _truckManager;

        public TruckController(ILogger<TruckController> logger,
            ITruckManager truckManager)
        {
            _logger = logger;
            _truckManager = truckManager;
        }

        // GET: api/<TruckController>
        [HttpGet]
        public async Task<IEnumerable<TruckResponceDTO>> GetAllTrucks()
        {
            return await _truckManager.GetAllTrucks();
        }

        // GET api/<TruckController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TruckController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TruckController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TruckController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
