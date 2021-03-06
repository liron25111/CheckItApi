using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    public partial class Student
    {
        public Student()
        {
            ClientsInGroups = new HashSet<ClientsInGroup>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(Account.Student))]
        public virtual Account IdNavigation { get; set; }
        [InverseProperty(nameof(ClientsInGroup.Client))]
        public virtual ICollection<ClientsInGroup> ClientsInGroups { get; set; }
    }
}
