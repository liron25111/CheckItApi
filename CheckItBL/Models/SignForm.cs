using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("SignForm")]
    public partial class SignForm
    {
        [Key]
        public int IdOfForm { get; set; }
        [Key]
        public int Account { get; set; }
        public TimeSpan SignatureTime { get; set; }

        [ForeignKey(nameof(Account))]
        [InverseProperty("SignForms")]
        public virtual Account AccountNavigation { get; set; }
        [ForeignKey(nameof(IdOfForm))]
        [InverseProperty(nameof(Form.SignForms))]
        public virtual Form IdOfFormNavigation { get; set; }
    }
}
