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
        public virtual DbSet<ModuleType> ModuleTypes { get; set; }

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
                entity.ToTable("AccessGroup");

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
                entity.ToTable("AccessGroupAssignment");

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
                entity.ToTable("AccessGroupModulePermission");

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
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.ToTable("Module");

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

                entity.HasOne(d => d.ParentModule)
                    .WithMany(p => p.InverseParentModule)
                    .HasForeignKey(d => d.ParentModuleId)
                    .HasConstraintName("FK_Module_Module");
            });

            modelBuilder.Entity<ModuleType>(entity =>
            {
                entity.ToTable("ModuleType");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
