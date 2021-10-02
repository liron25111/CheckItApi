using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckItBL.Models;

namespace CheckItApi.DTO
{
    public class AccountDTO
    {
        public AccountDTO(Account a)
        {
            this.Username = a.Username;
        }

        public string Username { get; set; }
        public string Pass { get; set; }
        public string SchoolName { get; set; }
        public string ClassId { get; set; }

        public string SchoolCode { get; set; }

        public string Email { get; set; }
    }
}
