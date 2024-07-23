using DLL.Data;
using DLL.Entities;
using DLL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository;

public class WordRepository : IWordRepository
{
	private DbSet<Word> Words { get; set; }

	public WordRepository(AppDbContext context)
    {
		Words = context.Set<Word>();
    }
    
    public async Task<IEnumerable<Word>> GetAllAsync()
    {
        var words = await Words
            .Include(w => w.WordRooms)
            .ToListAsync();
        return words;
    }

    public async Task<Word> GetByIdAsync(Guid id)
    {
        var words = await Words
            .Include(w => w.WordRooms)
            .FirstOrDefaultAsync(x => x.Id == id);
        return words!;
    }

    public async Task AddAsync(Word entity)
    {
        await Words.AddAsync(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
		var word = await Words.FindAsync(id);

		Words.Remove(word);
    }

    public void Update(Word entity)
    {
        Words.Update(entity);
    }
}