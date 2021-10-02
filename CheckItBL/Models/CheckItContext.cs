using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CheckItBL.Models
{
    public partial class CheckItContext : DbContext
    {
        public CheckItContext()
        {
        }

        public CheckItContext(DbContextOptions<CheckItContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClientsInGroup> ClientsInGroups { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<FormsOfGroup> FormsOfGroups { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<SignForm> SignForms { get; set; }
        public virtual DbSet<StaffMember> StaffMembers { get; set; }
        public virtual DbSet<StaffMemberOfGroup> StaffMemberOfGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = LAPTOP-RF95KAVF\\MSSQLSERVER01; Database=Confirmation; Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("group_groupid_primary");

                entity.Property(e => e.GroupId).ValueGeneratedNever();

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_schoolid_foreign");
            });

            modelBuilder.Entity<ClientsInGroup>(entity =>
            {
                entity.HasKey(e => new { e.ClientId, e.GroupId })
                    .HasName("clients_in_group_clientid_primary");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.Sender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("forms_sender_foreign");
            });

            modelBuilder.Entity<FormsOfGroup>(entity =>
            {
                entity.HasKey(e => new { e.IdOfGroup, e.FormId })
                    .HasName("formsofgroups_idofgroup_primary");

                entity.Property(e => e.FormId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.SchoolId)
                    .HasName("organizations_schoolid_primary");

                entity.Property(e => e.SchoolId).ValueGeneratedNever();

                entity.HasOne(d => d.ManagerNavigation)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.Manager)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("organizations_manager_foreign");
            });

            modelBuilder.Entity<SignForm>(entity =>
            {
                entity.HasKey(e => e.IdOfForm)
                    .HasName("sign_forms_idofform_primary");

                entity.Property(e => e.IdOfForm).ValueGeneratedNever();
            });

            modelBuilder.Entity<StaffMember>(entity =>
            {
                entity.HasOne(d => d.School)
                    .WithMany(p => p.StaffMembers)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("staff_member_schoolid_foreign");
            });

            modelBuilder.Entity<StaffMemberOfGroup>(entity =>
            {
                entity.HasKey(e => new { e.StaffMemberId, e.GroupId })
                    .HasName("staffmemberofgroup_staffmemberid_primary");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
