using DLL.Data;
using DLL.Entities;
using DLL.Interface;
using Microsoft.EntityFrameworkCore;


namespace DLL.Repository
{
	public class WordRoomRepository : IWordRoomRepository
	{
		private DbSet<WordRoom> WordRooms { get; set; }

		public WordRoomRepository(AppDbContext context)
		{
			WordRooms = context.Set<WordRoom>();
		}

		public async Task AddAsync(WordRoom entity)
		{
			await WordRooms.AddAsync(entity);
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			var entity = await WordRooms.FirstOrDefaultAsync(x => x.Id == id);

			WordRooms.Remove(entity!);
		}

		public async Task<IEnumerable<WordRoom>> GetAllAsync()
		{
			var entities = await WordRooms.ToListAsync();

			return entities;
		}

		public async Task<WordRoom> GetByIdAsync(Guid id)
		{
			var entity = await WordRooms.Include(wr => wr.Word)
										.Include(wr => wr.Room)
										.FirstOrDefaultAsync(wr => wr.Id == id);
			return entity!;
		}

		public void Update(WordRoom entity)
		{
			WordRooms.Update(entity);
		}
	}
}
