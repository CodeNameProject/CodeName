using System.ComponentModel.DataAnnotations;
using DLL.Enums;

namespace DLL.Entities;

public class Word : BaseEntity
{
    [Required] 
    [MaxLength(20)]
    public string WordName { get; set; } = null!;

    public ICollection<WordRoom>? WordRooms { get; set; }
}