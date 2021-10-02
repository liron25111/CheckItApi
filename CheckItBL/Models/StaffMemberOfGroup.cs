using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


#nullable disable

namespace CheckItBL.Models
{
    [Table("StaffMemberOfGroup")]
    public partial class StaffMemberOfGroup
    {
        [Key]
        public int StaffMemberId { get; set; }
        [Key]
        public int GroupId { get; set; }
    }
}
