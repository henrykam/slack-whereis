using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HenryKam.SlackWhereIs.Application;
using HenryKam.SlackWhereIs.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HenryKam.SlackWhereIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        ILocationRepository _locationRepository { get; set; }
        public LocationController(ILocationRepository locationRepository)
        {
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
        public IActionResult Post([FromBody] MeetingRoom location)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid meeting room data");
            }
            return Ok(_locationRepository.AddLocation(location));
        }

        // PUT: api/Location/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(_locationRepository.DeleteLocation(id));
        }
    }
}
