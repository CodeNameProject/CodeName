using DLL.Data;
using DLL.Entities;
using DLL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository;

public class UserRepository : IUserRepository
{

	private DbSet<User> Users { get; set; }

	public UserRepository(AppDbContext context)
    {
        Users = context.Set<User>();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await Users
            .Include(r => r.Room)
            .ToListAsync();

        return users;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await Users
            .Include(r => r.Room)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user!;
    }

    public async Task AddAsync(User entity)
    {
        await Users.AddAsync(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        Users.Remove(user);
    }

    public void Update(User entity)
    {
        Users.Update(entity);
    }
}