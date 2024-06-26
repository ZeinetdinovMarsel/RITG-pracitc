using Microsoft.EntityFrameworkCore;
using TMS.DataAccess.Entities;
using TMS.Core.Models;
namespace TMS.DataAccess
{
    public class TMSDbContext : DbContext
    {
        public TMSDbContext(DbContextOptions<TMSDbContext> options)
            : base(options)
        {
           
        }
        public DbSet<TskEntity> Tsks { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}