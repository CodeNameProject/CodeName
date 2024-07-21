namespace DLL.Entities;

public class Room : BaseEntity
{
    public ICollection<User>? Users { get; set; }
    public ICollection<WordRoom>? WordRooms { get; set; }
}