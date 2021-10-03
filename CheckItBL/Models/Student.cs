using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Index(nameof(FamilyId), Name = "FamilyId_unique", IsUnique = true)]
    public partial class Student
    {
        public Student()
        {
            ClientsInGroups = new HashSet<ClientsInGroup>();
        }

        [Key]
        public int StudentId { get; set; }
        public int FamilyId { get; set; }
        [Required]
        [StringLength(255)]
        public string StudentName { get; set; }

        public virtual Account Family { get; set; }
        [InverseProperty(nameof(ClientsInGroup.Client))]
        public virtual ICollection<ClientsInGroup> ClientsInGroups { get; set; }
    }
}
