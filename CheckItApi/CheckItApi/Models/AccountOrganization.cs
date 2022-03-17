using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItApi.Models
{
    public partial class AccountOrganization
    {
        [Key]
        public int AccountId { get; set; }
        [Key]
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("AccountOrganizations")]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("AccountOrganizations")]
        public virtual Organization Organization { get; set; }
    }
}
