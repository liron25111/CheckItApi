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
        public Account()
        {
            AccountOrganizations = new HashSet<AccountOrganization>();
            SignForms = new HashSet<SignForm>();
            Students = new HashSet<Student>();
        }

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

        [InverseProperty(nameof(AccountOrganization.Account))]
        public virtual ICollection<AccountOrganization> AccountOrganizations { get; set; }
        [InverseProperty(nameof(SignForm.AccountNavigation))]
        public virtual ICollection<SignForm> SignForms { get; set; }
        [InverseProperty(nameof(Student.Parent))]
        public virtual ICollection<Student> Students { get; set; }
    }
}
