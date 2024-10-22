using DAL.Entities;
using DAL.Enums;

namespace BLL.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public UserRole? UserRole { get; set; }
    public TeamColor? TeamColor { get; set; }
    public Guid RoomId { get; set; }
}