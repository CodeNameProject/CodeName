using System.ComponentModel.DataAnnotations;
using DLL.Enums;

namespace DLL.Entities;

public class Word
{
    public Guid Id { get; set; }

    [Required] 
    public string WordName { get; set; } = null!;

    public ICollection<WordRoom> WordRooms { get; set; }
}