using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	internal class RoomConfiguration : IEntityTypeConfiguration<Room>
	{
		public void Configure(EntityTypeBuilder<Room> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
					.ValueGeneratedOnAdd();
		}
	}
}
