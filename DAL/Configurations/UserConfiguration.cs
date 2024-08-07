using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
					.ValueGeneratedOnAdd();

			builder.HasOne(u => u.Room)
					.WithMany(r => r.Users)
					.HasForeignKey(u => u.RoomId)
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
		}
	}
}
