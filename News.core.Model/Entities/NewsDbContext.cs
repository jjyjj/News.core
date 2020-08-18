using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    public class NewsDbContext : DbContext
    {

        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<CommentsChild> CommentsChild { get; set; }

        public virtual DbSet<Focus> Focus { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<NewsToCategory> NewsToCategory { get; set; }
      


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=.;Database=NewsDb;uid=sa;pwd=123456");
            }
        }
    }
}
