using DLL.Enums;

namespace DLL.Entities;

public class WordRoom
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; }

    public WordColor Color { get; set; }
    
    public Guid WordId { get; set; }
    public Word Word { get; set; }
}