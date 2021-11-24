using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("Signform")]
    [Index(nameof(GroupId), Name = "Signform_groupid_unique", IsUnique = true)]
    [Index(nameof(PerentSignId), Name = "sign_forms_perentsignid_unique", IsUnique = true)]
    public partial class Signform
    {
        [Key]
        public int SignFormId { get; set; }
        public int IdOfForm { get; set; }
        public int PerentSignId { get; set; }
        public int GroupId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SignTime { get; set; }

        [ForeignKey(nameof(IdOfForm))]
        [InverseProperty(nameof(Form.Signforms))]
        public virtual Form IdOfFormNavigation { get; set; }
    }
}
