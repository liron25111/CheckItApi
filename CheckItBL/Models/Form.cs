using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CheckItBL.Models
{
    [Index(nameof(FormType), Name = "forms_formtype_index")]
    public partial class Form
    {
        public Form()
        {
            FormsOfGroups = new HashSet<FormsOfGroup>();
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
        public int Sender { get; set; }
        [Key]
        public int FormId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TimeSend { get; set; }

        [ForeignKey(nameof(Sender))]
        [InverseProperty(nameof(StaffMember.Forms))]
        public virtual StaffMember SenderNavigation { get; set; }
        [InverseProperty("IdOfFormNavigation")]
        public virtual Signform Signform { get; set; }
        [InverseProperty(nameof(FormsOfGroup.Form))]
        public virtual ICollection<FormsOfGroup> FormsOfGroups { get; set; }
    }
}
