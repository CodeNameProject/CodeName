using System.ComponentModel.DataAnnotations;
using DLL.Enums;

namespace DLL.Entities;

public class User
{
    public Guid Id { get; set; }
    [Required] 
    public string Name { get; set; } = null!;
    public  UserRole UserRole { get; set; }
    public TeamColor Team { get; set; }

    [Required]
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
}