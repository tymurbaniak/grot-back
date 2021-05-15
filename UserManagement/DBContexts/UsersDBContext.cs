using Microsoft.EntityFrameworkCore;
using UserManagement.DBModels;
using RefreshToken = UserManagement.ViewModels.RefreshToken;

namespace UserManagement.DBContexts
{
    public class UsersDBContext : DbContext
    {
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public UsersDBContext(DbContextOptions<UsersDBContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
            modelBuilder.Entity<UserGroup>().ToTable("UserGroups");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");

            // Configure Primary Keys  
            modelBuilder.Entity<UserGroup>().HasKey(ug => ug.Id).HasName("PK_UserGroups");
            modelBuilder.Entity<User>().HasKey(u => u.Id).HasName("PK_Users");
            modelBuilder.Entity<RefreshToken>().HasKey(t => t.Id).HasName("PK_RefreshToken");

            // Configure indexes  
            modelBuilder.Entity<UserGroup>().HasIndex(p => p.Name).IsUnique().HasName("Idx_Name");
            modelBuilder.Entity<User>().HasIndex(u => u.FirstName).HasName("Idx_FirstName");
            modelBuilder.Entity<User>().HasIndex(u => u.LastName).HasName("Idx_LastName");
            modelBuilder.Entity<User>().HasIndex(u => u.Name).HasName("Idx_Name").IsUnique(true);

            // Configure columns  
            modelBuilder.Entity<UserGroup>().Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UserGroup>().Property(ug => ug.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<UserGroup>().Property(ug => ug.CreationDateTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<UserGroup>().Property(ug => ug.LastUpdateDateTime).HasColumnType("datetime").IsRequired(false);

            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<User>().Property(u => u.Name).HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnType("nvarchar(60)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).HasColumnType("nvarchar(60)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.UserGroupId).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<User>().Property(u => u.CreationDateTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.LastUpdateDateTime).HasColumnType("datetime").IsRequired(false);

            modelBuilder.Entity<RefreshToken>().Property(rt => rt.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<RefreshToken>().Property(rt => rt.Token).HasColumnType("nvarchar(254)").IsRequired(false);
            modelBuilder.Entity<RefreshToken>().Property(rt => rt.Expires).HasColumnType("datetime");
            modelBuilder.Entity<RefreshToken>().Property(rt => rt.Created).HasColumnType("datetime");
            modelBuilder.Entity<RefreshToken>().Property(rt => rt.CreatedByIp).HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<RefreshToken>().Property(rt => rt.Revoked).HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<RefreshToken>().Property(rt => rt.RevokedByIp).HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<RefreshToken>().Property(rt => rt.ReplacedByToken).HasColumnType("nvarchar(254)").IsRequired(false);
            //modelBuilder.Entity<RefreshToken>().Property(rt => rt.UserId).HasColumnType("int");

            // Configure relationships  
            modelBuilder.Entity<User>().HasOne<UserGroup>().WithMany()
                .HasPrincipalKey(ug => ug.Id).HasForeignKey(u => u.UserGroupId)
                .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Users_UserGroups");
            //modelBuilder.Entity<RefreshToken>().HasOne<User>().WithMany()
            //    .HasPrincipalKey(u => u.Id).HasForeignKey(rt => rt.UserId)
            //    .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_RefreshToken_User");
            //modelBuilder.Entity<User>().HasMany<RefreshToken>().WithOne()
            //    .HasPrincipalKey(u => u.Id).HasForeignKey(rt => rt.Id)
            //    .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_User_RefreshToken");
            //modelBuilder.Entity<RefreshToken>().HasOne<User>().WithMany()
            //    .HasPrincipalKey(u => u.Id).HasForeignKey(rt => rt.Id)
            //    .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_RefreshToken_User");
        }
    }
}
