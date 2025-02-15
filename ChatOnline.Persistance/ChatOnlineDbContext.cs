﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatOnline.Domain.Common;
using System.Reflection;
using ChatOnline.Domain.Entities;
using ChatOnline.Application.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace ChatOnline.Persistance
{
    public class ChatOnlineDbContext : DbContext, IChatOnlineDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        public ChatOnlineDbContext(DbContextOptions<ChatOnlineDbContext> options, IDateTime dateTime, ICurrentUserService currentUserService) : base(options)
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // method indicates all EF CORE configuration (classes which implements IEntityTypeConfiguration)

            modelBuilder.SeedData();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        entry.Entity.StatusId = 1;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _currentUserService.UserId;
                        entry.Entity.Modified = _dateTime.Now;
                        entry.Entity.StatusId = 1;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.ModifiedBy = _currentUserService.UserId;
                        entry.Entity.Modified = _dateTime.Now;
                        entry.Entity.InactivatedBy = _currentUserService.UserId;
                        entry.Entity.Inactivated = _dateTime.Now;
                        entry.Entity.StatusId = 0;
                        entry.State = EntityState.Modified; // Show entity framework that we do not want to delete entity
                        break;

                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
