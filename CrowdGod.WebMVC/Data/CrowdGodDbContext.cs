using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdGod.Data
{
    public class CrowdGodDbContext : DbContext
    {
        public CrowdGodDbContext(DbContextOptions<CrowdGodDbContext> options)
            : base(options)
        { }
        public static CrowdGodDbContext Create(DbContextOptions<CrowdGodDbContext> options)
        {
            return new CrowdGodDbContext(options);
        }

        public DbSet<Question>? Questions { get; set; }
        public DbSet<Answer>? Answers { get; set; }
        public DbSet<Reply>? Replies { get; set; }
        public DbSet<Tag>? Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Question>()
                .HasMany(a => a.Answers)
                .WithOne(a => a.Question);

            builder.Entity<Answer>()
                .HasMany(a => a.Replies)
                .WithOne(a => a.Answer);

            builder.Entity<Question>()
                .HasMany(p => p.Tags)
                .WithMany(p => p.Questions);

            base.OnModelCreating(builder);
        }
    }
}
