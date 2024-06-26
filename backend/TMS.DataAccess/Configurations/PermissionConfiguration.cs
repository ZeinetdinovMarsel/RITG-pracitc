using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml.Linq;
using TMS.DataAccess.Entities;
using TMS.Core.Enums;

namespace TMS.DataAccess.Configurations
{
    public partial class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {


        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.HasKey(r => r.Id);

            var permissions = Enum
                .GetValues<Permission>()
                .Select(p => new RoleEntity
                {
                    Id = (int)p,
                    Name = p.ToString()
                });
            builder.HasData(permissions);
        }
    }
}
