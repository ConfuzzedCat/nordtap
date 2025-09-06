using backend.Data.Context;
using backend.Data.Services.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

namespace backend.Data.Services;

public class RoomService : IRoomService
{
    private readonly DataContext _context;
    private readonly ILogger<RoomService> _logger;

    public RoomService(DataContext context, ILogger<RoomService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Room>> Create(Room entity)
    {
        await _context.RoomsDb.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;        
    }

    public async Task<Maybe<Room>> Read(string entityId)
    {
        return await _context.RoomsDb.FindAsync(entityId);
    }

    public async Task<Result<Room>> Update(Room entity)
    {
        var newEntity = _context.RoomsDb.Update(entity);
        await _context.SaveChangesAsync();
        return newEntity.Entity;
    }

    public async Task<Result<Room>> Delete(string entityId)
    {
        var room =  await Read(entityId);
        if (room.HasNoValue)
        {
            throw new NullReferenceException("Room was not found.");
        }
        var removedRoom = _context.RoomsDb.Remove(room.Value).Entity;
        await _context.SaveChangesAsync();
        return removedRoom;
    }

    public async Task<Result<List<Room>>> GetAllRoomsUserIsIn(User user)
    {
        return await _context.RoomsDb
            .Where(r => r.Users.Contains(user))
            .Include(r => r.Owner)
            .ToListAsync();
    }
}