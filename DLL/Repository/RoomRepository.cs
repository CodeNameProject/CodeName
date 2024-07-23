using DLL.Data;
using DLL.Entities;
using DLL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository;

public class RoomRepository : IRoomRepository
{
	private DbSet<Room> Rooms { get; set; }

	public RoomRepository(AppDbContext context)
	{
		Rooms = context.Set<Room>();
	}

	public async Task<IEnumerable<Room>> GetAllAsync()
	{
		var rooms = await Rooms
			.Include(x => x.WordRooms)
			.Include(c => c.Users)
			.ToListAsync();
		
		return rooms;
	}

	public async Task<Room> GetByIdAsync(Guid id)
	{
		var room = await Rooms
			.Include(x => x.WordRooms)
			.ThenInclude(w => w.Word)
			.Include(c => c.Users)
			.FirstOrDefaultAsync(c => c.Id == id);

		return room!;
	}

	public async Task AddAsync(Room entity)
	{
		await Rooms.AddAsync(entity);
	}

	public async Task DeleteByIdAsync(Guid id)
	{
		var room = await Rooms.FindAsync(id);
		Rooms.Remove(room);
	}

	public void Update(Room entity)
	{
		Rooms.Update(entity);
	}
}