using DLL.Interface;
using DLL.Repository;

namespace DLL.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        RoomRepository = new RoomRepository(_context);
        UserRepository = new UserRepository(_context);
        WordRepository = new WordRepository(_context);
    }
    public IRoomRepository RoomRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IWordRepository WordRepository { get; set; }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}