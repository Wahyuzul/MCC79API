using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : GeneralController<Booking>
    {
        public BookingController(IBookingRepository repository) : base(repository)
        {
        }
    }
}
