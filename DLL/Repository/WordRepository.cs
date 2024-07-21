using DLL.Data;
using DLL.Entities;
using DLL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository;

public class WordRepository : IWordRepository
{
    private readonly AppDbContext _context;

    public WordRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Word>> GetAllAsync()
    {
        var words = await _context.Words
            .Include(w => w.WordRooms)
            .ToListAsync();
        return words;
    }

    public async Task<Word> GetByIdAsync(Guid id)
    {
        var words = await _context.Words
            .Include(w => w.WordRooms)
            .FirstOrDefaultAsync(x => x.Id == id);
        return words!;
    }

    public async Task AddAsync(Word entity)
    {
        await _context.Words.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var word = await GetByIdAsync(id);
        _context.Words.Remove(word);
        await _context.SaveChangesAsync();
    }

    public void Update(Word entity)
    {
        _context.Words.Update(entity);
        _context.SaveChanges();
    }
}