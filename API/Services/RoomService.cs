using API.Contracts;
using API.DTOs.Rooms;
using API.DTOs.Universities;
using API.Models;

namespace API.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IEnumerable<GetRoomDto>? GetRoom()
        {
            var rooms = _roomRepository.GetAll();
            if (!rooms.Any())
            {
                return Enumerable.Empty<GetRoomDto>(); // No rooms found
            }

            var toDto = rooms.Select(room =>
                                        new GetRoomDto
                                        {
                                            Guid = room.Guid,
                                            Name = room.Name,
                                            Floor = room.Floor,
                                            Capacity = room.Capacity
                                        }).ToList();

            return toDto; // Rooms found
        }

        public IEnumerable<GetRoomDto>? GetRoom(string name)
        {
            var rooms = _roomRepository.GetByName(name);
            if (!rooms.Any())
            {
                return Enumerable.Empty<GetRoomDto>(); // No rooms found
            }

            var toDto = rooms.Select(room =>
                                        new GetRoomDto
                                        {
                                            Guid = room.Guid,
                                            Name = room.Name,
                                            Floor = room.Floor,
                                            Capacity = room.Capacity
                                        }).ToList();

            return toDto; // Rooms found
        }

        public GetRoomDto? GetRoom(Guid guid)
        {
            var room = _roomRepository.GetByGuid(guid);
            if (room is null)
            {
                return null; // Room not found
            }

            var toDto = new GetRoomDto
            {
                Guid = room.Guid,
                Name = room.Name,
                Floor = room.Floor,
                Capacity = room.Capacity
            };

            return toDto; // Room found
        }

        public GetRoomDto? CreateRoom(NewRoomDto newRoomDto)
        {
            var room = new Room
            {
                Name = newRoomDto.Name,
                Floor = newRoomDto.Floor,
                Capacity = newRoomDto.Capacity,
                Guid = new Guid(),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdRoom = _roomRepository.Create(room);
            if (createdRoom is null)
            {
                return null; // Room not created
            }

            var toDto = new GetRoomDto
            {
                Guid = createdRoom.Guid,
                Name = createdRoom.Name,
                Floor = createdRoom.Floor,
                Capacity = createdRoom.Capacity
            };

            return toDto; // Room created
        }

        public int UpdateRoom(UpdateRoomDto updateRoomDto)
        {
            var isExist = _roomRepository.IsExist(updateRoomDto.Guid);
            if (!isExist)
            {
                return -1; // Room not found
            }

            var getRoom = _roomRepository.GetByGuid(updateRoomDto.Guid);

            var room = new Room
            {
                Guid = updateRoomDto.Guid,
                Name = updateRoomDto.Name,
                Floor = updateRoomDto.Floor,
                Capacity = updateRoomDto.Capacity,
                ModifiedDate = DateTime.Now,
                CreatedDate = getRoom!.CreatedDate
            };

            var isUpdate = _roomRepository.Update(room);
            if (!isUpdate)
            {
                return 0; // Room not updated
            }

            return 1;
        }

        public int DeleteRoom(Guid guid)
        {
            var isExist = _roomRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // Room not found
            }

            var room = _roomRepository.GetByGuid(guid);
            var isDelete = _roomRepository.Delete(room!);
            if (!isDelete)
            {
                return 0; // Room not deleted
            }

            return 1;
        }
    }

}
