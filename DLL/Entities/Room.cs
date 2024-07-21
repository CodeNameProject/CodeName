namespace DLL.Entities;

public class Room
{
    public Guid Id { get; set; }

    public ICollection<User>? Users { get; set; }
    public ICollection<WordRoom>? WordRooms { get; set; }
}