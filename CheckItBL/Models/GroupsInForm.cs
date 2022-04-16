using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("GroupsInForm")]
    public partial class GroupsInForm
    {
        [Key]
        public int GroupId { get; set; }
        [Key]
        public int FormId { get; set; }

        [ForeignKey(nameof(FormId))]
        [InverseProperty("GroupsInForms")]
        public virtual Form Form { get; set; }
        [ForeignKey(nameof(GroupId))]
        [InverseProperty(nameof(Class.GroupsInForms))]
        public virtual Class Group { get; set; }
    }
}
