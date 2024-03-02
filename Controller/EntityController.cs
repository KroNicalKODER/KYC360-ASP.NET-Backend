using KYC360.Interfaces;
using KYC360.Models;
using Microsoft.AspNetCore.Mvc;

namespace KYC360.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EntityController : Controller
    {
        private readonly IEntityRepository _entityRepository;
        public EntityController(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Entity>))]
        public IActionResult GetEntities()
        {
            var entities = _entityRepository.GetEntities();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(entities);
        }

        [HttpGet("{eid}")]
        [ProducesResponseType(200, Type = typeof(Entity))]
        [ProducesResponseType(400)]
        public IActionResult GetById(string eid)
        {
            var entity = _entityRepository.GetById(eid);

            if (!ModelState.IsValid || entity==null)
            {
                return BadRequest(ModelState);
            }
            return Ok(entity);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Entity))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Entity entity)
        {
            if (entity == null)
            {
                return BadRequest("Entity is null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _entityRepository.CreateEntity(entity);
            return Ok(entity);

        }

        [HttpPut("{eid}")]
        [ProducesResponseType(200, Type = typeof(Entity))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEntity(string eid, [FromBody] Entity updatedEntity)
        {
            if (updatedEntity == null)
            {
                return BadRequest("Updated entity is null");
            }

            var existingEntity = _entityRepository.GetById(eid);

            if (existingEntity == null)
            {
                return NotFound(); // Return a 404 Not Found response
            }

            await _entityRepository.UpdateEntity(existingEntity, updatedEntity);

            return Ok(existingEntity);
        }

        [HttpDelete]
        [ProducesResponseType(204)]     //NO Content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteEntity(string eid)
        {
            var entity = _entityRepository.GetById(eid);

            if (entity == null)
            {
                return NotFound();
            }
            await _entityRepository.DeleteEntity(entity);

            return NoContent();
        }
        
        [HttpGet("search/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Entity>))]
        public IActionResult GetEntities([FromQuery] string search)
        {
            ICollection<Entity> entities;

            if (!string.IsNullOrEmpty(search))
            {
                entities = _entityRepository.SearchEntities(search);
            }
            else
            {
                entities = _entityRepository.GetEntities();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(entities);
        }

        [HttpGet("pagination")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Entity>))]
        public async Task<IActionResult> GetEntitiesByPage([FromQuery] int page = 1, [FromQuery] int pageSize = 2)
        {
            var entities = await _entityRepository.GetEntitiesByPage(page, pageSize);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(entities);
        }

        [HttpGet("advanced")]
        public async Task<IActionResult> AdvancedSearch(
            [FromQuery] string gender,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] List<string> countries)
        {
            var result = await _entityRepository.AdvancedSearch(gender, startDate, endDate, countries);
            return Ok(result);
        }
    }
}
