﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItApi.Models
{
    [Index(nameof(FormType), Name = "forms_formtype_index")]
    public partial class Form
    {
        public Form()
        {
            SignForms = new HashSet<SignForm>();
        }

        [StringLength(255)]
        public string FormType { get; set; }
        [Required]
        [Column("topic")]
        [StringLength(255)]
        public string Topic { get; set; }
        [Column("massageBody")]
        [StringLength(255)]
        public string MassageBody { get; set; }
        public int StatusOfTheMessage { get; set; }
        [Key]
        public int FormId { get; set; }
        public int GroupId { get; set; }
        public TimeSpan Time { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty(nameof(Class.Forms))]
        public virtual Class Group { get; set; }
        [InverseProperty(nameof(SignForm.IdOfFormNavigation))]
        public virtual ICollection<SignForm> SignForms { get; set; }
    }
}