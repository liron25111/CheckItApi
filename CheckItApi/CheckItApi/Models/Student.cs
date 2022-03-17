using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItApi.Models
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
        public int ParentId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Account.Students))]
        public virtual Account Parent { get; set; }
        [InverseProperty(nameof(ClientsInGroup.Client))]
        public virtual ICollection<ClientsInGroup> ClientsInGroups { get; set; }
    }
}
