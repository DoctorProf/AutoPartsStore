using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AutoPartsStore.DataBase
{
    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }
        public static Context Instance { get; } = new Context();
        public virtual DbSet<AutoPart> AutoPart { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AutoPart>()
                .HasMany(e => e.Order)
                .WithRequired(e => e.AutoPart)
                .HasForeignKey(e => e.ID_AutoPart)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.User)
                .WithRequired(e => e.Role)
                .HasForeignKey(e => e.IDRole)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Order)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.ID_User)
                .WillCascadeOnDelete(false);
        }
    }
}
