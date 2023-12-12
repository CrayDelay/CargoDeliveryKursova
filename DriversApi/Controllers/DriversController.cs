using Drivers.BLL.DTOs.Requests;
using Drivers.BLL.DTOs.Responses;
using Drivers.BLL.Managers.Contracts;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Entities.HelpModels;
using Drivers.DAL.EF.Helpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Drivers.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ILogger<DriversController> _logger;
        private IDriversManager _driversManager;

        public DriversController(ILogger<DriversController> logger,
            IDriversManager driversManager)
        {
            _logger = logger;
            _driversManager= driversManager;
        }
        
        /// <summary>
        /// Get Driver by Id
        /// </summary>
        [HttpGet("{id}", Name = "GetInformationAboutDriver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FullDriverResponceDTO>> GetFullInfoAboutDriver(int id, 
            CancellationToken cancellationToken)
        {
            var result = await _driversManager.GetFullInfoAboutDriver(id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get all drivers from database
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("alldrivers")]
        public async Task<ActionResult<IEnumerable<ShortDriverResponceDTO>>> GetAllDriversAsync(CancellationToken cancellationToken)
        {

            var result = await _driversManager.GetListOfAllDrivers(cancellationToken);
            if (result == null)
            {
                _logger.LogInformation($"Records are absent in database");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Кeceived drivers from the database!");
                return Ok(result);
            }
        }

        /// <summary>
        /// Get paganated drivers
        /// </summary>
        /// <param name="driverParameters">Model for paginated drivers</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("alldriverspaginated")]
        public async Task<ActionResult<PagedList<EFDriver>>> GetAllDriversPaginatedAsync([FromQuery] DriverParameters driverParameters, 
            CancellationToken cancellationToken)
        {
            if (!driverParameters.ValidYearRange)
            {
                return BadRequest("Max year of birth cannot be less than min year of birth");
            }
            try
            {
                var drivers = await _driversManager.GetPaginatedDrivers(driverParameters, cancellationToken);
                var metadata = new
                {
                    drivers.TotalCount,
                    drivers.PageSize,
                    drivers.CurrentPage,
                    drivers.TotalPages,
                    drivers.HasNext,
                    drivers.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInformation($"Returned {drivers.TotalCount} owners from database.");

                return Ok(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Запит не відпрацював, щось пішло не так! - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Add driver
        /// </summary>
        /// <param name="drv">Information about driver - MiniDriverReqDTO</param>
        /// <param name="validator">FluentValidation validator of MiniDriverReqDTO </param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult> PostDriverAsync([FromBody] MiniDriverReqDTO drv,
            [FromServices] IValidator<MiniDriverReqDTO> validator,
            CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"==> ADD ==> API Input : {drv.GetType().Name}");
            ValidationResult validationResult = validator.Validate(drv);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var created_driver = await _driversManager.AddDriverToSystemAsync(drv, cancellationToken);
            this._logger.LogInformation($"==> Added driver : {{ Driver: {created_driver.LastName}-{created_driver.Id}}}");
            return CreatedAtAction(nameof(GetFullInfoAboutDriver), new { id = created_driver.Id }, created_driver);
        }

        /// <summary>
        /// Update Driver by Id
        /// </summary>
        /// <param name="id">Id of driver</param>
        /// <param name="drv">Information about driver</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDriverAsync(int id,
            [FromBody] MiniDriverReqDTO drv,
            [FromServices] IValidator<MiniDriverReqDTO> validator,
            CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"==> UPDATE ==> API Input :  Driver with id = {id}, {drv.GetType().Name}");
            ValidationResult validationResult = validator.Validate(drv);
            if (id <= 0 || id!=drv.id)
            {
                return BadRequest($"Invalid id: {id}");
            }
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _driversManager.UpdateDriverInSystemAsync(id, drv, cancellationToken));
        }

        /// <summary>
        /// Delete Driver By Id
        /// </summary>
        /// <param name="id">Identifier of driver</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDriverByIdAsync(int id, 
            CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"==> DELETE ==> API Input :  Id of Driver: {id}");
            if (id <= 0)
            {
                return BadRequest($"Invalid id: {id}");
            }
            await _driversManager.DeleteDriverFromSystemAsync(id, cancellationToken);
            return Ok($"Deleted! Id of deleted entity is = {id}");
        }
    }
}
