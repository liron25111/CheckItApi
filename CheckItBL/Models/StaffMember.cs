using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("StaffMember")]
    [Index(nameof(Email), Name = "staffmember_email_unique", IsUnique = true)]
    public partial class StaffMember
    {
        public StaffMember()
        {
            Classes = new HashSet<Class>();
            Organizations = new HashSet<Organization>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string MemberName { get; set; }
        public int? SchoolId { get; set; }
        [Required]
        [StringLength(255)]
        public string Pass { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [ForeignKey(nameof(SchoolId))]
        [InverseProperty(nameof(Organization.StaffMembers))]
        public virtual Organization School { get; set; }
        [InverseProperty(nameof(Class.StaffMemberOfGroupNavigation))]
        public virtual ICollection<Class> Classes { get; set; }
        [InverseProperty(nameof(Organization.Manager))]
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
