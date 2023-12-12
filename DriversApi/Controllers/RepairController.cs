using Drivers.BLL.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Drivers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairController : ControllerBase
    {
        private readonly ILogger<RepairController> _logger;
        private IRepairManager _repairManager;

        public RepairController(ILogger<RepairController> logger,
            IRepairManager repairManager)
        {
            _logger = logger;
            _repairManager = repairManager;
        }


        // GET: api/<RepairController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RepairController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RepairController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RepairController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RepairController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
