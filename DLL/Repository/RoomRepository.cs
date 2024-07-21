using DLL.Data;
using DLL.Entities;
using DLL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        var rooms = await _context.Rooms
            .Include(x => x.WordRooms)
            .Include(c => c.Users)
            .ToListAsync();
        return rooms;
    }

    public async Task<Room> GetByIdAsync(Guid id)
    {
        var room = await _context.Rooms
            .Include(x => x.WordRooms)
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Id == id);
        return room!;
    }

    public async Task AddAsync(Room entity)
    {
        await _context.Rooms.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var room = await GetByIdAsync(id);
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }

    public void Update(Room entity)
    {
        _context.Rooms.Update(entity);
        _context.SaveChangesAsync();
    }
}