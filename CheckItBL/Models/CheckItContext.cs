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
        public virtual DbSet<GroupsInForm> GroupsInForms { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<SignForm> SignForms { get; set; }
        public virtual DbSet<StaffMember> StaffMembers { get; set; }
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
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccountOrganization>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.OrganizationId })
                    .HasName("accountorganizations_accountid_primary");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountOrganizations)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_Id_foreign");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.AccountOrganizations)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AccountOrganizations_OrganizationId_foreign");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("group_groupid_primary");

                entity.HasOne(d => d.StaffMemberOfGroupNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.StaffMemberOfGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_staffmemberofgroup_foreign");
            });

            modelBuilder.Entity<ClientsInGroup>(entity =>
            {
                entity.HasKey(e => new { e.ClientId, e.GroupId })
                    .HasName("clientsingroup_clientid_primary");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientsInGroups)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ClientsInGroup_ClientId_foreign");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ClientsInGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ClientsInGroup_GroupId_foreign");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasOne(d => d.SentByStaffMemebrNavigation)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.SentByStaffMemebr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Form_SentByStaffMember");
            });

            modelBuilder.Entity<GroupsInForm>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.FormId })
                    .HasName("groupsInForm_primary");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.GroupsInForms)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FormId_foreign");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupsInForms)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GroupId_foreign");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.SchoolId)
                    .HasName("organizations_schoolid_primary");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("Organizations_ManagerId_foreign");
            });

            modelBuilder.Entity<SignForm>(entity =>
            {
                entity.HasKey(e => new { e.IdOfForm, e.Account })
                    .HasName("signForm_idofform_primary");

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.SignForms)
                    .HasForeignKey(d => d.Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_Idform_foreign");

                entity.HasOne(d => d.IdOfFormNavigation)
                    .WithMany(p => p.SignForms)
                    .HasForeignKey(d => d.IdOfForm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SignForm_IdOfForm_foreign");
            });

            modelBuilder.Entity<StaffMember>(entity =>
            {
                entity.HasOne(d => d.School)
                    .WithMany(p => p.StaffMembers)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("staffmember_schoolid_foreign");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("students_Id_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}