using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    public partial class FormsOfGroup
    {
        [Key]
        public int IdOfGroup { get; set; }
        [Key]
        public int FormId { get; set; }

        [ForeignKey(nameof(FormId))]
        [InverseProperty("FormsOfGroups")]
        public virtual Form Form { get; set; }
        [ForeignKey(nameof(IdOfGroup))]
        [InverseProperty(nameof(Class.FormsOfGroups))]
        public virtual Class IdOfGroupNavigation { get; set; }
    }
}
