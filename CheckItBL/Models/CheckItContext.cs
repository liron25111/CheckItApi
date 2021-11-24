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
        public virtual DbSet<AccountOrganization> AccountOrganizations { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClientsInGroup> ClientsInGroups { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<FormsOfGroup> FormsOfGroups { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Signform> Signforms { get; set; }
        public virtual DbSet<StaffMember> StaffMembers { get; set; }
        public virtual DbSet<StaffMemberOfGroup> StaffMemberOfGroups { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = localhost\\SQLEXPRESS; Database=Confirmation; Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AccountOrganization>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.OragnizationId })
                    .HasName("AccountOrganizationsPK");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountOrganizations)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AOA");

                entity.HasOne(d => d.Oragnization)
                    .WithMany(p => p.AccountOrganizations)
                    .HasForeignKey(d => d.OragnizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AOO");
            });

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

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientsInGroups)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Clients_in_Group_ClientId_foreign");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ClientsInGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Clients_in_Group_GroupId_foreign");
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

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.FormsOfGroups)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FormsOfGroups_IdOfForm_foreign");

                entity.HasOne(d => d.IdOfGroupNavigation)
                    .WithMany(p => p.FormsOfGroups)
                    .HasForeignKey(d => d.IdOfGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FormsOfGroups_IdOfGroup_foreign");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.SchoolId)
                    .HasName("organizations_schoolid_primary");

                entity.Property(e => e.SchoolId).ValueGeneratedNever();

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Account_Organizations");

                entity.HasOne(d => d.ManagerNavigation)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StaffMemberId_foreign");
            });

            modelBuilder.Entity<Signform>(entity =>
            {
                entity.HasOne(d => d.IdOfFormNavigation)
                    .WithMany(p => p.Signforms)
                    .HasForeignKey(d => d.IdOfForm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Sign_Forms_IdOfForm_foreign");
            });

            modelBuilder.Entity<StaffMember>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.StaffMember)
                    .HasForeignKey<StaffMember>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AccountIdStaffMember_foreign");

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

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.StaffMemberOfGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StaffMemberOfGroup_GroupId_foreign");

                entity.HasOne(d => d.StaffMember)
                    .WithMany(p => p.StaffMemberOfGroups)
                    .HasForeignKey(d => d.StaffMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StaffMemberOfGroup_Id_foreign");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Students_familyId_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
