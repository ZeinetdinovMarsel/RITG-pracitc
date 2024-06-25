using Microsoft.EntityFrameworkCore;
using TMS.DataAccess.Entities;
namespace TMS.DataAccess
{
    public class TMSDbContext : DbContext
    {
        public TMSDbContext(DbContextOptions<TMSDbContext> options)
            : base(options)
        {
        }
        public DbSet<TskEntity> Tsks { get; set; }
    }
}