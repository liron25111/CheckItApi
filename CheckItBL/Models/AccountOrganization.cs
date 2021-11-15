using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    public partial class AccountOrganization
    {
        [Key]
        public int AccountId { get; set; }
        [Key]
        public int OragnizationId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("AccountOrganizations")]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(OragnizationId))]
        [InverseProperty(nameof(Organization.AccountOrganizations))]
        public virtual Organization Oragnization { get; set; }
    }
}
