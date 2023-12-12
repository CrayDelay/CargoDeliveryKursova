using Drivers.BLL.DTOs.Responses;
using Drivers.BLL.Managers;
using Drivers.BLL.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Drivers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private ICompanyManager _companyManager;

        public CompanyController(ILogger<CompanyController> logger,
            ICompanyManager companyManager)
        {
            _logger = logger;
            _companyManager = companyManager;
        }

        // GET: api/<CompanyController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CompanyController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CompanyResponceDTO>> GetById(int id)
        {
            try
            {
                var result = await _companyManager.GetCompanyById(id);
                if (result == null)
                {
                    _logger.LogInformation($"Company із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали Company з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Потрібно дивитись!");
            }
        }

        // POST api/<CompanyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CompanyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CompanyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
