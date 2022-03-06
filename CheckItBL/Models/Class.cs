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
            Forms = new HashSet<Form>();
        }

        [Required]
        [StringLength(255)]
        public string ClassName { get; set; }
        public int StaffMemberOfGroup { get; set; }
        [Key]
        public int GroupId { get; set; }
        [Required]
        [StringLength(255)]
        public string ClassYear { get; set; }

        [ForeignKey(nameof(StaffMemberOfGroup))]
        [InverseProperty(nameof(StaffMember.Classes))]
        public virtual StaffMember StaffMemberOfGroupNavigation { get; set; }
        [InverseProperty(nameof(ClientsInGroup.Group))]
        public virtual ICollection<ClientsInGroup> ClientsInGroups { get; set; }
        [InverseProperty(nameof(Form.Group))]
        public virtual ICollection<Form> Forms { get; set; }
    }
}
