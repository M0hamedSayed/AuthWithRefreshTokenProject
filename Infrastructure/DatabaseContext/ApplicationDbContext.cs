﻿using System.Reflection.Emit;
using System.Reflection;
using Data.Entities.Identity;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        #region constructors
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        #endregion
        #region Methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        #endregion

        #region Tables
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserRefreshTokens> UserRefreshTokens { get; set; }
        #endregion
    }
}
