using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("Account")]
    [Index(nameof(Email), Name = "account_email_unique", IsUnique = true)]
    public partial class Account
    {
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        public string Pass { get; set; }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public bool IsActiveStudent { get; set; }
    }
}
