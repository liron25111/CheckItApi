using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("Staff_Member")]
    public partial class StaffMember
    {
        public StaffMember()
        {
            Forms = new HashSet<Form>();
            Organizations = new HashSet<Organization>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string MemberName { get; set; }
        public int PositionName { get; set; }
        public int SchoolId { get; set; }

        [ForeignKey(nameof(SchoolId))]
        [InverseProperty(nameof(Organization.StaffMembers))]
        public virtual Organization School { get; set; }
        [InverseProperty(nameof(Form.SenderNavigation))]
        public virtual ICollection<Form> Forms { get; set; }
        [InverseProperty(nameof(Organization.ManagerNavigation))]
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
