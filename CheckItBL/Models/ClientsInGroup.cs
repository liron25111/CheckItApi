using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Table("Clients_in_Group")]
    public partial class ClientsInGroup
    {
        [Key]
        public int ClientId { get; set; }
        [Key]
        public int GroupId { get; set; }
    }
}
