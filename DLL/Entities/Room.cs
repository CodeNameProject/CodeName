namespace DLL.Entities;

public class Room : BaseEntity
{
    public bool IsStarted { get; set; }
    public ICollection<User>? Users { get; set; }
    public ICollection<WordRoom>? WordRooms { get; set; }
}