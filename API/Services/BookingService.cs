using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using API.Repositories;
using API.Utilities;

namespace API.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IEmployeeRepository employeeRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
            _roomRepository = roomRepository;
        }

        public IEnumerable<GetBookingDto>? GetBooking()
        {
            var bookings = _bookingRepository.GetAll();
            if (!bookings.Any())
            {
                return null; // No Booking  found
            }

            var toDto = bookings.Select(booking =>
                                                new GetBookingDto
                                                {
                                                    Guid = booking.Guid,
                                                    StartDate = booking.StartDate,
                                                    EndDate = booking.EndDate,
                                                    Status = booking.Status,
                                                    Remarks = booking.Remarks,
                                                    RoomGuid = booking.RoomGuid,
                                                    EmployeeGuid = booking.EmployeeGuid
                                                }).ToList();

            return toDto; // Booking found
        }

        public GetBookingDto? GetBooking(Guid guid)
        {
            var booking = _bookingRepository.GetByGuid(guid);
            if (booking is null)
            {
                return null; // booking not found
            }

            var toDto = new GetBookingDto
            {
                Guid = booking.Guid,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status,
                Remarks = booking.Remarks,
                RoomGuid = booking.RoomGuid,
                EmployeeGuid = booking.EmployeeGuid
            };

            return toDto; // bookings found
        }

        public GetBookingDto? CreateBooking(NewBookingDto newBookingDto)
        {
            
            var booking = new Booking
            {
                Guid = new Guid(),
                StartDate = newBookingDto.StartDate,
                EndDate = newBookingDto.EndDate,
                Status = newBookingDto.Status,
                Remarks = newBookingDto.Remarks,
                RoomGuid = newBookingDto.RoomGuid,
                EmployeeGuid = newBookingDto.EmployeeGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdBooking = _bookingRepository.Create(booking);
            if (createdBooking is null)
            {
                return null; // Booking not created
            }

            var toDto = new GetBookingDto
            {
                Guid = createdBooking.Guid,
                StartDate = newBookingDto.StartDate,
                EndDate = newBookingDto.EndDate,
                Status = newBookingDto.Status,
                Remarks = newBookingDto.Remarks,
                RoomGuid = newBookingDto.RoomGuid,
                EmployeeGuid = newBookingDto.EmployeeGuid,
            };

            return toDto; // Booking created
        }

        public int UpdateBooking(UpdateBookingDto updateBookingDto)
        {
            var isExist = _bookingRepository.IsExist(updateBookingDto.Guid);
            if (!isExist)
            {
                return -1; // Booking not found
            }

            var getBooking = _bookingRepository.GetByGuid(updateBookingDto.Guid);

            var booking = new Booking
            {
                Guid = updateBookingDto.Guid,
                StartDate = updateBookingDto.StartDate,
                EndDate = updateBookingDto.EndDate,
                Status = updateBookingDto.Status,
                Remarks = updateBookingDto.Remarks,
                RoomGuid = updateBookingDto.RoomGuid,
                EmployeeGuid = updateBookingDto.EmployeeGuid,
                ModifiedDate = DateTime.Now,
                CreatedDate = getBooking!.CreatedDate
            };

            var isUpdate = _bookingRepository.Update(booking);
            if (!isUpdate)
            {
                return 0; // Booking not updated
            }

            return 1;
        }

        public int DeleteBooking(Guid guid)
        {
            var isExist = _bookingRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // Booking not found
            }

            var booking = _bookingRepository.GetByGuid(guid);
            var isDelete = _bookingRepository.Delete(booking!);
            if (!isDelete)
            {
                return 0; // Booking not deleted
            }

            return 1;
        }

        public ICollection<BookingDetailsDto>? BookingDetails()
        {
            var bookings = _bookingRepository.GetAll();
            if (bookings == null)
            {
                return null; // No Booking  found
            }

            var employees = _employeeRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            var getBookingDetails = (
                from booking in bookings
                join employee in employees on booking.EmployeeGuid equals employee.Guid
                join room in rooms on booking.RoomGuid equals room.Guid
                select new BookingDetailsDto
                {
                    Guid = booking.Guid,
                    BookedNik = employee.NIK,
                    BookedBy = employee.FirstName + " " + employee.LastName,
                    RoomName = room.Name,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    Status = booking.Status,
                    Remarks = booking.Remarks
                }
            ).ToList();

            return getBookingDetails;
        }

        public BookingDetailsDto? BookingDetailByGuid(Guid guid)
        {
            var bookings = BookingDetails();

            var bookByGuid = bookings!.FirstOrDefault(book => book.Guid == guid);

            return bookByGuid;
        }

        public ICollection<BookedRoomDto>? GetBookedRoom()
        {
            var bookings = _bookingRepository.GetAll();
            if (bookings == null)
            {
                return null; // No Booking  found
            }

            var employees = _employeeRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            var getBookedDetail = (
                from booking in bookings
                join employee in employees on booking.EmployeeGuid equals employee.Guid
                join room in rooms on booking.RoomGuid equals room.Guid
                where booking.StartDate <= DateTime.Now.Date && booking.EndDate <= DateTime.Now
                select new BookedRoomDto
                {
                    Guid = booking.Guid,
                    BookedBy = employee.FirstName + " " + employee.LastName,
                    RoomName = room.Name,
                    Status = booking.Status,
                    Floor = room.Floor

                }
            ).ToList();
            if (!getBookedDetail.Any())
            {
                return null;
            }

            return getBookedDetail;
        }

        public IEnumerable<BookingLengthDto>? BookingDuration()
        {
            var bookings = _bookingRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            var entities = (from booking in bookings
                            join room in rooms on booking.RoomGuid equals room.Guid
                            select new
                            {
                                guid = room.Guid,
                                startDate = booking.StartDate,
                                endDate = booking.EndDate,
                                roomName = room.Name
                            }).ToList();

            var bookingDurations = new List<BookingLengthDto>();

            foreach (var entity in entities)
            {
                TimeSpan duration = entity.endDate - entity.startDate;

                // Count the number of weekends within the duration
                int totalDays = (int)duration.TotalDays;
                int weekends = 0;

                for (int i = 0; i <= totalDays; i++)
                {
                    var currentDate = entity.startDate.AddDays(i);
                    if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        weekends++;
                    }
                }

                // Calculate the duration without weekends
                TimeSpan bookingLength = duration - TimeSpan.FromDays(weekends);

                var bookingDurationDto = new BookingLengthDto
                {
                    RoomGuid = entity.guid,
                    RoomName = entity.roomName,
                    BookingLength = bookingLength
                };

                bookingDurations.Add(bookingDurationDto);
            }

            return bookingDurations;
        }
    }
}
