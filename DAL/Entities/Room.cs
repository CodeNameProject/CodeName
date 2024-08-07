namespace DAL.Entities;

public class Room : BaseEntity
{
    public bool IsStarted { get; set; }
    public ICollection<User> Users { get; set; } = null!;
    public ICollection<WordRoom> WordRooms { get; set; } = null!;
}