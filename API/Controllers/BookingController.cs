using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _repository;

        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var booking = _repository.GetAll();

            if (!booking.Any())
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var booking = _repository.GetByGuid(guid);
            if (booking is null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        public IActionResult Create(Booking booking)
        {
            var createdBooking = _repository.Create(booking);
            return Ok(createdBooking);
        }

        [HttpPut]
        public IActionResult Update(Booking booking)
        {
            var isUpdated = _repository.Update(booking);
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
