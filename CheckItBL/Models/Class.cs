using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("Class")]
    public partial class Class
    {
        public Class()
        {
            ClientsInGroups = new HashSet<ClientsInGroup>();
            FormsOfGroups = new HashSet<FormsOfGroup>();
            StaffMemberOfGroups = new HashSet<StaffMemberOfGroup>();
        }

        [Required]
        [StringLength(255)]
        public string ClassName { get; set; }
        [Key]
        public int GroupId { get; set; }
        public int SchoolId { get; set; }
        public int YearTime { get; set; }

        [ForeignKey(nameof(SchoolId))]
        [InverseProperty(nameof(Organization.Classes))]
        public virtual Organization School { get; set; }
        [InverseProperty(nameof(ClientsInGroup.Group))]
        public virtual ICollection<ClientsInGroup> ClientsInGroups { get; set; }
        [InverseProperty(nameof(FormsOfGroup.IdOfGroupNavigation))]
        public virtual ICollection<FormsOfGroup> FormsOfGroups { get; set; }
        [InverseProperty(nameof(StaffMemberOfGroup.Group))]
        public virtual ICollection<StaffMemberOfGroup> StaffMemberOfGroups { get; set; }
    }
}
