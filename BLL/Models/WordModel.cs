using DLL.Entities;

namespace BLL.Models;

public class WordModel
{
    public Guid Id { get; set; }
    public string? WordName { get; set; }
    public ICollection<WordRoom>? WordRooms { get; set; }
    
}