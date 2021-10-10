using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CheckItBL.Models
{
    partial class CheckItContext
    {
        public Account Login(string email, string pass)
        {
            Account user = this.Accounts
                .Where(u => u.Email == email && u.Pass == pass).FirstOrDefault();

            return user;
        }
    }
}
