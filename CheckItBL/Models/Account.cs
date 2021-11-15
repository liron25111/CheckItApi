using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("Account")]
    [Index(nameof(Email), Name = "account_email_unique", IsUnique = true)]
    public partial class Account
    {
        public Account()
        {
            AccountOrganizations = new HashSet<AccountOrganization>();
            Organizations = new HashSet<Organization>();
        }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        public string Pass { get; set; }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [InverseProperty("IdNavigation")]
        public virtual StaffMember StaffMember { get; set; }
        [InverseProperty("IdNavigation")]
        public virtual Student Student { get; set; }
        [InverseProperty(nameof(AccountOrganization.Account))]
        public virtual ICollection<AccountOrganization> AccountOrganizations { get; set; }
        [InverseProperty(nameof(Organization.Manager))]
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
