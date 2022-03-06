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
        [InverseProperty(nameof(StaffMember.Organizations))]
        public virtual StaffMember Manager { get; set; }
        [InverseProperty(nameof(AccountOrganization.Organization))]
        public virtual ICollection<AccountOrganization> AccountOrganizations { get; set; }
        [InverseProperty(nameof(StaffMember.School))]
        public virtual ICollection<StaffMember> StaffMembers { get; set; }
    }
}
