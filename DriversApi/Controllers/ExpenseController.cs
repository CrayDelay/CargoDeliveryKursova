using Drivers.BLL.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;


namespace Drivers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ILogger<ExpenseController> _logger;
        private IExpenseManager _expenseManager;

        public ExpenseController(ILogger<ExpenseController> logger,
            IExpenseManager expenseManager)
        {
            _logger = logger;
            _expenseManager = expenseManager;
        }


        // GET: api/<ExpenseController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ExpenseController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ExpenseController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ExpenseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ExpenseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
