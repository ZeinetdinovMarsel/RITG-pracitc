﻿using Microsoft.EntityFrameworkCore;
using TMS.DataAccess.Entities;
using Microsoft.Extensions.Options;
namespace TMS.DataAccess
{
    public class TMSDbContext(
        DbContextOptions<TMSDbContext> options,
        IOptions<AuthorizationOptions> authOptions) : DbContext(options)
    {
        public DbSet<TskEntity> Tsks { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<TskHistoryEntity> TskHistories { get; set; }
        public DbSet<UserRoleEntity> UserRoleEntity { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TMSDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        }
    }
}