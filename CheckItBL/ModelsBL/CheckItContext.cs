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
            List<int> classIds = this.ClientsInGroups.Where(x => x.ClientId == id).Select(s => s.GroupId).ToList();
            List<int> formIds = new List<int>();
            foreach(int i in classIds)
            {
                List<int> current = this.GroupsInForms.Where(x => x.GroupId == i).Select(s => s.FormId).ToList();
                foreach(int formId in current)
                {
                    formIds.Add(formId);
                }
            }
            List<Form> forms = new List<Form>();
            foreach(int i in formIds)
                    forms.Add(this.Forms.FirstOrDefault(x => x.FormId == i));
            return new List<Form>(new HashSet<Form>(forms));
        }
        public List<Form> GetFormsByStaffMember(int id)
        {
            //List<SignForm> signForms = SignForms.Where(s => s.IdOfFormNavigation.Group.StaffMemberOfGroup == id).ToList<SignForm>();
            //List<Form> forms = new List<Form>();
            //foreach (SignForm form in signForms)
            //{
            //    forms.Add(form.IdOfFormNavigation);
            //}
            //return forms;

            List<Form> forms = this.Forms.Where(x => x.SentByStaffMemebr == id).ToList();
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

        public Form AddForm(Form f)
        {
            this.Forms.Add(f);
            this.SaveChanges();
            return f;
        }

        public bool PostForm(Form f, List<int> ids)
        {
            try
            {
                Form form = this.AddForm(f);
                foreach (int i in ids)
                {
                    this.GroupsInForms.Add(new GroupsInForm() { GroupId = i, FormId = form.FormId });
                }
                this.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }


        }

        public List<Class> GetFormGroups(int formId)
        {
            List<Class> groups = this.GroupsInForms.Where(x => x.FormId == formId).Select(s => s.Group).ToList();
            return groups;
        }

        public List<Class> GetMyGroupsStudent(int id)
        {
            List<Class> groups = this.ClientsInGroups.Where(x => x.ClientId == id).Select(x => x.Group).ToList();
            return groups;
        }
        public List<Student> GetAccountsFromGroup(int classId)
        {
            List<Student> accounts = this.ClientsInGroups.Where(x => x.GroupId == classId).Select(a => a.Client).ToList();
            return accounts;
        }
        public List<Class> GetMyGroupsStaff(int staffId)
        {
            List<Class> classes = this.Classes.Where(x => x.StaffMemberOfGroup == staffId).ToList();
            return classes;
        }

        public List<Account> GetAccountsSigned(int formId)
        {
            List<Account> accounts = this.SignForms.Where(x => x.IdOfForm == formId).Select(a => a.AccountNavigation).ToList();
            return accounts;
        }
        public List<Account> GetWhoDidntSign(int formId)
        {
            List<Class> gif = this.GroupsInForms.Where(x => x.FormId == formId).Select(a => a.Group).ToList();
            List<Account> assigned = new List<Account>();
            foreach(Class c in gif)
            {
                List<Student> accounts = c.ClientsInGroups.Select(x => x.Client).ToList();
                foreach (Student s in accounts)
                    assigned.Add(this.Accounts.FirstOrDefault(w => w.Id == s.Id));
            }
            List<Account> signed = this.GetAccountsSigned(formId);
            List<Account> didntSign = new List<Account>();
            foreach (Account acc in assigned)
            {
                if(!signed.Contains(acc))
                    didntSign.Add(acc);
            }
            return didntSign;
        }
    }
}
