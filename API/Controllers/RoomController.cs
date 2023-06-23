using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/room")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _repository;

        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var room = _repository.GetAll();

            if (!room.Any())
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid) 
        {
            var room = _repository.GetByGuid(guid);
            if(room is null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        public IActionResult Create(Room room)
        {
            var createdRoom = _repository.Create(room);
            return Ok(createdRoom);
        }

        [HttpPut]
        public IActionResult Update(Room room)
        {
            var isUpdated = _repository.Update(room);
            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok(isUpdated);
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var isDeleted = _repository.Delete(guid);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
