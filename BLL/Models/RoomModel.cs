using DLL.Entities;

namespace BLL.Models;

public class RoomModel
{
    public Guid Id { get; set; }
    public ICollection<UserModel>? Users { get; set; }
    public ICollection<WordRoomModel>? WordRooms { get; set; }
    public bool IsStarted { get; set; }
}