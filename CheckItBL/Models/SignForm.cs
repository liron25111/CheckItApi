using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("Sign_Forms")]
    [Index(nameof(GroupId), Name = "sign_forms_groupid_unique", IsUnique = true)]
    [Index(nameof(PerentSignId), Name = "sign_forms_perentsignid_unique", IsUnique = true)]
    public partial class SignForm
    {
        [Key]
        public int IdOfForm { get; set; }
        public int PerentSignId { get; set; }
        public int GroupId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SignTime { get; set; }
    }
}
