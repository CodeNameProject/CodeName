using DAL.Configurations;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
	public DbSet<Word> Words { get; set; }
    public DbSet<WordRoom> WordRooms { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new RoomConfiguration());
		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new WordConfiguration());
		modelBuilder.ApplyConfiguration(new WordRoomConfiguration());
	}
}