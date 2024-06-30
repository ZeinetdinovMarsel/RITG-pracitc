using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Core.Models;
using TMS.DataAccess.Entities;

namespace TMS.DataAccess.Configurations.Core
{
    public class TskConfiguration : IEntityTypeConfiguration<TskEntity>
    {
        public void Configure(EntityTypeBuilder<TskEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(b => b.CreatorId)
            .IsRequired();

            builder.Property(b => b.AssignedUserId)
            .IsRequired();

            builder.Property(b => b.Title)
            .HasMaxLength(Tsk.MAX_TITLE_LENGTH)
            .IsRequired();

            builder.Property(b => b.Comment)
            .IsRequired();

            builder.Property(b => b.Priority)
            .IsRequired();

            builder.Property(b => b.Status)
            .IsRequired();

            builder.Property(b => b.StartDate)
            .IsRequired();

            builder.Property(b => b.EndDate)
            .IsRequired();

            builder.Property(b => b.AcceptDate)
           .IsRequired();

            builder.Property(b => b.FinishDate)
            .IsRequired();

        }
    }
}
