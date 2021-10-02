using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    public partial class FormsOfGroup
    {
        [Key]
        public int IdOfGroup { get; set; }
        [Key]
        public int FormId { get; set; }
    }
}
