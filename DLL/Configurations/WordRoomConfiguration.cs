using DLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations
{
	internal class WordRoomConfiguration : IEntityTypeConfiguration<WordRoom>
	{
		public void Configure(EntityTypeBuilder<WordRoom> builder)
		{
			builder.HasKey(wr => new {wr.WordId, wr.RoomId});

			builder.HasOne(wr => wr.Room)
				.WithMany(r => r.WordRooms)
				.HasForeignKey(r => r.RoomId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(wr => wr.Word)
				.WithMany(w => w.WordRooms)
				.HasForeignKey(r => r.WordId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}