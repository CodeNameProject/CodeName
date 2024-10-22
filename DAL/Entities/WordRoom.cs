using DAL.Enums;

namespace DAL.Entities;

public class WordRoom : BaseEntity
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;

	public WordColor? Color { get; set; }

    public bool IsUncovered { get; set; }

    public Guid WordId { get; set; }
    public Word Word { get; set; } = null!;
}