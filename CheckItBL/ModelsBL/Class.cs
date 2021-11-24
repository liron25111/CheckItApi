using System;
using System.Collections.Generic;
using System.Text;

namespace CheckItBL.Models
{
    public partial class Class
    {
        public int SumPeople()
        {
            return this.ClientsInGroups.Count;
        }
    }
}
