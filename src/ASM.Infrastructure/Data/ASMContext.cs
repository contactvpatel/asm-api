using ASM.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASM.Infrastructure.Data
{
    public partial class ASMContext : DbContext
    {
        public ASMContext()
        {
        }

        public ASMContext(DbContextOptions<ASMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessGroup> AccessGroups { get; set; }
        public virtual DbSet<AccessGroupAssignment> AccessGroupAssignments { get; set; }
        public virtual DbSet<AccessGroupModulePermission> AccessGroupModulePermissions { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleHierarchy> ModuleHierarchies { get; set; }
        public virtual DbSet<ModuleType> ModuleTypes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(local);Database=ASM;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccessGroup>(entity =>
            {
                entity.ToTable("AccessGroup", "ASM");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccessGroupAssignment>(entity =>
            {
                entity.ToTable("AccessGroupAssignment", "ASM");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AccessGroup)
                    .WithMany(p => p.AccessGroupAssignments)
                    .HasForeignKey(d => d.AccessGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessGroupAssignment_AccessGroup");
            });

            modelBuilder.Entity<AccessGroupModulePermission>(entity =>
            {
                entity.ToTable("AccessGroupModulePermission", "ASM");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AccessGroup)
                    .WithMany(p => p.AccessGroupModulePermissions)
                    .HasForeignKey(d => d.AccessGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessGroupModulePermission_AccessGroup");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.AccessGroupModulePermissions)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessGroupModulePermission_Module");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.AccessGroupModulePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessGroupModulePermission_Permission");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.ToTable("Module", "ASM");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ModuleType)
                    .WithMany(p => p.Modules)
                    .HasForeignKey(d => d.ModuleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Module_ModuleType");
            });

            modelBuilder.Entity<ModuleHierarchy>(entity =>
            {
                entity.ToTable("ModuleHierarchy", "ASM");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ModuleHierarchyModules)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleHierarchy_Module");

                entity.HasOne(d => d.ParentModule)
                    .WithMany(p => p.ModuleHierarchyParentModules)
                    .HasForeignKey(d => d.ParentModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleHierarchy_ParentModule");
            });

            modelBuilder.Entity<ModuleType>(entity =>
            {
                entity.ToTable("ModuleType", "ASM");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permission", "ASM");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
