using CSharpFunctionalExtensions;
using Shared.Data.Entities;

namespace backend.Data.Services.Interfaces;

public interface IRoomService : ICrud<string, Room>
{
    Task<Result<List<Room>>> GetAllRoomsUserIsIn(User user); 
}