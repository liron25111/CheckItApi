using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    public partial class Organization
    {
        public Organization()
        {
            AccountOrganizations = new HashSet<AccountOrganization>();
            Classes = new HashSet<Class>();
            StaffMembers = new HashSet<StaffMember>();
        }

        [Key]
        public int SchoolId { get; set; }
        public int ManagerId { get; set; }
        [Required]
        [StringLength(255)]
        public string OrganizationName { get; set; }
        public int MashovSchoolId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        [InverseProperty(nameof(Account.Organizations))]
        public virtual Account Manager { get; set; }
        [ForeignKey(nameof(ManagerId))]
        [InverseProperty(nameof(StaffMember.Organizations))]
        public virtual StaffMember ManagerNavigation { get; set; }
        [InverseProperty(nameof(AccountOrganization.Oragnization))]
        public virtual ICollection<AccountOrganization> AccountOrganizations { get; set; }
        [InverseProperty(nameof(Class.School))]
        public virtual ICollection<Class> Classes { get; set; }
        [InverseProperty(nameof(StaffMember.School))]
        public virtual ICollection<StaffMember> StaffMembers { get; set; }
    }
}
