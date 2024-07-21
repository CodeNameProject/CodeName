using DLL.Data;
using DLL.Entities;
using DLL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _context.Users
            .Include(r => r.Room)
            .ToListAsync();
        return users;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _context.Users
            .Include(r => r.Room)
            .FirstOrDefaultAsync(u => u.Id == id);
        return user!;
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public void Update(User entity)
    {
        _context.Users.Update(entity);
        _context.SaveChanges();
    }
}