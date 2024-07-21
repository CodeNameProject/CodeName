using DLL.Entities;
using DLL.Enums;

namespace BLL.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public  UserRole UserRole { get; set; }
    public TeamColor Team { get; set; }
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
}