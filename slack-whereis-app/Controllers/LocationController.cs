using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HenryKam.SlackWhereIs.Application;
using HenryKam.SlackWhereIs.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HenryKam.SlackWhereIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        ILocationRepository _locationRepository { get; set; }
        private ILogger _logger { get; }
        public LocationController(ILogger<LocationController> logger, ILocationRepository locationRepository)
        {
            _logger = logger;
            _locationRepository = locationRepository;
        }

        // GET: api/Location
        [HttpGet]
        public IEnumerable<Location> Get()
        {
            return _locationRepository.GetLocations();
        }

        // GET: api/Location/5
        [HttpGet("{name}", Name = "GetByName")]
        public IEnumerable<Location> GetByName(string name)
        {
            return _locationRepository.GetLocationByName(name);
        }

        // GET: api/Location/5
        [HttpGet("{id}", Name = "GetById")]
        public Location GetById(string id)
        {
            return _locationRepository.GetLocationById(id);
        }

        // POST: api/Location
        [HttpPost]
        [Route("meetingroom")]
        public IActionResult Post([FromBody] MeetingRoom location)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid meeting room data");
                return BadRequest("Invalid meeting room data");
            }

            if(_locationRepository.GetLocationByName(location.Name).Any())
            {
                _logger.LogInformation($"Location name already exists: {location.Name}");
                return BadRequest($"Location name already exists: {location.Name}");
            }

            return Ok(_locationRepository.AddLocation(location));
        }

        [HttpPost]
        [Route("employee")]
        public IActionResult Post([FromBody] Employee location)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid employee data");
                return BadRequest("Invalid employee data");
            }

            if (_locationRepository.GetLocationByName(location.Name).Any())
            {
                _logger.LogInformation($"Location name already exists: {location.Name}");
                return BadRequest($"Location name already exists: {location.Name}");
            }

            return Ok(_locationRepository.AddLocation(location));
        }

        [HttpPost]
        [Route("department")]
        public IActionResult Post([FromBody] Department location)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid employee data");
                return BadRequest("Invalid employee data");
            }

            if (_locationRepository.GetLocationByName(location.Name).Any())
            {
                _logger.LogInformation($"Location name already exists: {location.Name}");
                return BadRequest($"Location name already exists: {location.Name}");
            }

            return Ok(_locationRepository.AddLocation(location));
        }

        // PUT: api/Location/5
        [HttpPut("meetingroom/{id}")]
        //[Route("meetingroom")]
        public IActionResult Put(string id, [FromBody] MeetingRoom location)
        {
            try
            {
                //location.Id = Convert.ToInt64(id);

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Invalid meeting room data received");
                    return BadRequest("Invalid meeting room data");
                }

                if (_locationRepository.GetLocationById(id) == null)
                {
                    return Post(location);
                }
                else
                {
                    return Ok(_locationRepository.UpdateLocation(location));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid id or location: {ex.Message}");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(_locationRepository.DeleteLocation(id));
        }
    }
}
