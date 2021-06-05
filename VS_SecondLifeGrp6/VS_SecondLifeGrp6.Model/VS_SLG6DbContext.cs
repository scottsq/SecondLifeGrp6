using Microsoft.EntityFrameworkCore;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Model
{
    public class VS_SLG6DbContext : DbContext
    {
        public DbSet<Message> Message { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductRating> ProductRating { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<Proposal> Proposal { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRating> UserRating { get; set; }

        public VS_SLG6DbContext(DbContextOptions<VS_SLG6DbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=j5zntocs2dn6c3fj.chr7pe7iynqr.eu-west-1.rds.amazonaws.com;port=3306;database=cn9t75q26l1rxt3u;uid=dmgvbxe88p399tes;password=m58y72f7j44ka7zp;TreatTinyAsBoolean=false");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
