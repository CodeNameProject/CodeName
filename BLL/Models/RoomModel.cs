using DLL.Entities;

namespace BLL.Models;

public class RoomModel
{
    public Guid Id { get; set; }
    public ICollection<User>? Users { get; set; }
    public ICollection<WordRoom>? WordRooms { get; set; }
}