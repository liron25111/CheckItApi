using CheckItBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckItApi.DTO
{
    public class StudentSignDTO
    {
        public Student s;
        public bool signed;
        public StudentSignDTO(Student s,bool b)
        {
            this.s = s;
            signed = b;
        }
        public StudentSignDTO()
        {

        }
    }
}
