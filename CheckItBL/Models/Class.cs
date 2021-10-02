using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ConsoleApp40.Models
{
    [Table("Class")]
    public partial class Class
    {
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
    }
}
