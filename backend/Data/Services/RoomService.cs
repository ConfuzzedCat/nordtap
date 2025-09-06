using backend.Data.Context;
using backend.Data.Services.Interfaces;
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

    public async Task<Room> Create(Room entity)
    {
        await _context.RoomsDb.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;        
    }

    public async Task<Room?> Read(string entityId)
    {
        return await _context.RoomsDb.FindAsync(entityId);
    }

    public async Task<Room> Update(Room entity)
    {
        var newEntity = _context.RoomsDb.Update(entity);
        await _context.SaveChangesAsync();
        return newEntity.Entity;
    }

    public async Task<Room> Delete(string entityId)
    {
        var room =  await Read(entityId);
        if (room == null)
        {
            throw new NullReferenceException("Room was not found.");
        }
        var removedRoom = _context.RoomsDb.Remove(room).Entity;
        await _context.SaveChangesAsync();
        return removedRoom;
    }

    public async Task<List<Room>> GetAllRoomsUserIsIn(User user)
    {
        return await _context.RoomsDb
            .Where(r => r.Users.Contains(user))
            .Include(r => r.Owner)
            .ToListAsync();
    }
}