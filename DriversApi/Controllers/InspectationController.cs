using Drivers.BLL.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Drivers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectationController : ControllerBase
    {
        private readonly ILogger<InspectationController> _logger;
        private IInspectationManager _inspectationManager;

        public InspectationController(ILogger<InspectationController> logger,
            IInspectationManager inspectationManager)
        {
            _logger = logger;
            _inspectationManager = inspectationManager;
        }



        // GET: api/<InspectationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<InspectationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InspectationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InspectationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InspectationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
