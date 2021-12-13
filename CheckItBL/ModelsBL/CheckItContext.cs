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
         
        public void ChangePass(string email, string pass)
        {
            Account a = this.Accounts.FirstOrDefault(a => a.Email == email);

            if (a != null)
            {
                a.Pass = pass;

                this.SaveChanges();
            }
        }
        public Account GetAccountByEmail(string email) => this.Accounts.FirstOrDefault(a => a.Email == email);
        public (int, int) GetFormSigns(int formId)
        {
            Form form = Forms.Find(formId);
            int signed = form.Signforms.Count;
            int total = 0;
            foreach(FormsOfGroup f in form.FormsOfGroups)
            {
                total += f.IdOfGroupNavigation.SumPeople();
            }
            return (total, signed);
        }


        public List<Form> GetFormsByAccount(int id)
        {
            var res = from cInG in this.ClientsInGroups
                      join grp in this.FormsOfGroups on cInG.GroupId equals (grp.IdOfGroup)
                      where cInG.ClientId == id
                      select grp.Form;
            return res.ToList();
        }

    }
}
