using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("StaffMember")]
    public partial class StaffMember
    {
        public StaffMember()
        {
            Forms = new HashSet<Form>();
            Organizations = new HashSet<Organization>();
            StaffMemberOfGroups = new HashSet<StaffMemberOfGroup>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string MemberName { get; set; }
        public int PositionName { get; set; }
        public int SchoolId { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(Account.StaffMember))]
        public virtual Account IdNavigation { get; set; }
        [ForeignKey(nameof(SchoolId))]
        [InverseProperty(nameof(Organization.StaffMembers))]
        public virtual Organization School { get; set; }
        [InverseProperty(nameof(Form.SenderNavigation))]
        public virtual ICollection<Form> Forms { get; set; }
        [InverseProperty(nameof(Organization.Manager))]
        public virtual ICollection<Organization> Organizations { get; set; }
        [InverseProperty(nameof(StaffMemberOfGroup.StaffMember))]
        public virtual ICollection<StaffMemberOfGroup> StaffMemberOfGroups { get; set; }
    }
}
