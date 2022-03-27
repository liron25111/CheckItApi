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
        public StaffMember Login2(string email, string pass)
        {
            StaffMember user = this.StaffMembers
                .Where(u => u.Email == email && u.Pass == pass).FirstOrDefault();

            return user;
        }

        public bool IsSigned(string email, int formId)
        {
            return this.SignForms.Where(s => s.AccountNavigation.Email == email && s.IdOfFormNavigation.FormId == formId) != null;
        }
        public Account GetAccount(string email) => Accounts.Where(a => a.Email == email).FirstOrDefault();
        public void AddAccount(Account a)
        {
            Accounts.Add(a);
            this.SaveChanges();
        }
        public int Signs(int formId)
        {
            return this.SignForms.Count(s => s.IdOfFormNavigation.FormId == formId);
        }
        public Form GetForm(int formId)
        {
            return this.Forms.Where(f => f.FormId == formId).FirstOrDefault();
        }
        public Student GetStudent(int id)
        {
            return this.Students.Where(s => s.Id == id).FirstOrDefault();
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
        public void ChangePassStaffMember(string email, string pass)
        {
            StaffMember a = this.StaffMembers.FirstOrDefault(a => a.Email == email);

            if (a != null)
            {
                a.Pass = pass;

                this.SaveChanges();
            }
        }
        public Account GetAccountByEmail(string email) => this.Accounts.FirstOrDefault(a => a.Email == email);

        public StaffMember GetStaffMemberByEmail(string email) => StaffMembers.Where(s => s.Email == email).FirstOrDefault();

        public List<Form> GetFormsByAccount(int id)
        {
            List<SignForm> signForms = SignForms.Where(s => s.AccountNavigation.Id == id).ToList<SignForm>();
            List<Form> forms = new List<Form>();
            foreach (SignForm form in signForms)
            {
                forms.Add(form.IdOfFormNavigation);
            }
            return forms;
        }
        public List<Form> GetFormsByStaffMember(int id)
        {
            List<SignForm> signForms = SignForms.Where(s => s.IdOfFormNavigation.Group.StaffMemberOfGroup == id).ToList<SignForm>();
            List<Form> forms = new List<Form>();
            foreach (SignForm form in signForms)
            {
                forms.Add(form.IdOfFormNavigation);
            }
            return forms;
        }
        public Class CreateClass(int id, List<Account> accounts, string className)
        {
            Class c = new Class() { ClassName = className, StaffMemberOfGroup = id, ClassYear = DateTime.Now.Year.ToString() };
            Classes.Add(c);
            this.SaveChanges();
            foreach (Account a in accounts)
            {
                ClientsInGroups.Add(new ClientsInGroup() { ClientId = a.Id, GroupId = c.GroupId });
            }
            this.SaveChanges();
            return c;
        }
        public void AddStudent(Student s)
        {
            Students.Add(s);
            this.SaveChanges();
        }
        public List<Class> GetClasses()
        {
            return this.Classes.ToList<Class>();
        }
    }
}
