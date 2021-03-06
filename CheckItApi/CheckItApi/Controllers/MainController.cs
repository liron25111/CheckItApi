using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CheckItBL.Models;
using CheckItApi.DTO;
using System.IO;
using Spire.Xls;
using SendGridLib;
using Newtonsoft.Json;
using System.Text;
using System.IO;

using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace CheckItApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class MainController : ControllerBase
    {
        #region Add connection to the db context using dependency injection
        CheckItContext context;
        public MainController(CheckItContext context)
        {
            this.context = context;
        }
        #endregion
        [Route("SignUpAccount")]
        [HttpPost]
        //public AccountDTO SignUpAccount([FromBody] AccountDTO newAccount)
        //{

        //}
        [Route("Login")]
        [HttpGet]
        public Account Login([FromQuery] string email, [FromQuery] string pass)
        {
            Account user = context.Login(email, pass);

            //Check user name and password
            if (user != null)
            {
                HttpContext.Session.SetObject("theUser", user);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                //Important! Due to the Lazy Loading, the user will be returned with all of its contects!!
                return user;
            }
            else
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        [Route("Alert")]
        [HttpGet]
        public bool Alert([FromQuery] int formId)
        {
            StaffMember user = HttpContext.Session.GetObject<StaffMember>("staffMember");
            if(user != null)
            {
                Form f = context.GetForm(formId);
                if(f != null)
                {
                    List<Tuple<Student, bool>> tuples = context.GetSignedStudentsInForm(formId);
                    foreach(Tuple<Student, bool> tuple in tuples)
                    {
                        if (!tuple.Item2)
                        {
                            MailSender.SendEmail("Check It", context.GetAccount(tuple.Item1.Id).Email, tuple.Item1.Name, "Sign Your form", $"You Have Form That you hav'nt signed yet : {f.Topic}", "");

                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                    return true;

                }
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return false;
        }
        [Route("Login2")]
        [HttpGet]
        public StaffMember Login2([FromQuery] string email, [FromQuery] string pass)
        {
            StaffMember user = context.Login2(email, pass);

            //Check user name and password
            if (user != null)
            {
                HttpContext.Session.SetObject("staffMember", user);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                //Important! Due to the Lazy Loading, the user will be returned with all of its contects!!
                return user;
            }
            else
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }


        [Route("ResetPass")]
        [HttpGet]
        public Account ResetPass([FromQuery] string pass, [FromQuery] string Email)
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }
            Account account = context.GetAccountByEmail(Email);

            if (user != null)
            {
                context.ChangePass(user.Email, pass);
                MailSender.SendEmail("Check It", $"{ account.Email}", $"{ account.Username}", "Your Password Changed", $"Your New Password is {account.Pass} ","");

                return user;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        [Route("ResetPassStaffMember")]
        [HttpGet]
        public StaffMember ResetPassStaffMember([FromQuery] string pass, [FromQuery] string Email)
        {
            StaffMember user = HttpContext.Session.GetObject<StaffMember>("staffMember");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }
            StaffMember account = context.GetStaffMemberByEmail(Email);

            if (user != null)
            {
                context.ChangePassStaffMember(user.Email, pass);
                MailSender.SendEmail("Check It", $"{ account.Email}", $"{ account.MemberName}", "Your Password Changed", $"Your New Password is {account.Pass} ","");

                return user;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        [Route("ForgotPassword")]
        [HttpGet]
        public bool ForgotPassword([FromQuery] string Email)
        {
            bool succeed = false;
            Account account = context.GetAccountByEmail(Email);
            StaffMember staffMember = context.GetStaffMemberByEmail(Email);
            if (account != null)
            {
                MailSender.SendEmail("Check It", $"{ account.Email}", $"{ account.Username}", "Password Recovery", $"Your Password is {account.Pass} ","");

                succeed = true;
            }
            else if (staffMember != null)
            {
                MailSender.SendEmail("Check It", $"{ staffMember.Email}", $"{ staffMember.MemberName}", "Password Recovery", $"Your Password is {staffMember.Pass} ","");
                succeed = true;
            }
            return succeed;
        }
        [Route("Signs")]
        [HttpGet]
        public int Signs([FromQuery] int formId)
        {
            if (context.GetForm(formId) == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return 0;
            }
            return context.Signs(formId);
        }
        [Route("GetSignPeople")]
        [HttpGet]
        public bool GetSignPeople([FromQuery] int formId, [FromQuery] string Email)
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return false;
            }

            if (user != null)
            {
                return context.IsSigned(Email, formId);
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }
        }
        [Route("CheckIfSigned")]
        [HttpGet]
        public bool CheckIfSigned([FromQuery] int formId, [FromQuery] int clientid)
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return false;
            }

            if (user != null)
            {
                 context.FormSigned(clientid, formId);
                return true;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }
        }
        [Route("LogOut")]
        [HttpGet]
        public bool LogOut()
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            StaffMember staffMember = HttpContext.Session.GetObject<StaffMember>("staffMember");
            if (user == null && staffMember == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return false;
            }
            HttpContext.Session.SetObject("staffMember", null);
            HttpContext.Session.SetObject("theUser", null);
            return true;


        }

        //צריך לעשות פעולה כזאת לstaffMember
        [Route("GetForms")]
        [HttpGet]
        public List<Form> GetForms([FromQuery] int clientId)
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            StaffMember staffMember = HttpContext.Session.GetObject<StaffMember>("staffMember");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null && staffMember == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }

            if (user != null)
            {
                return context.GetFormsByAccount(clientId);
            }
            else if (staffMember != null)
            {
                return context.GetFormsByStaffMember(clientId);
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }
        [Route("GetFormsStaffMember")]
        [HttpGet]
        public List<Form> GetFormsStaffMember([FromQuery] int clientId)
        {
            StaffMember user = HttpContext.Session.GetObject<StaffMember>("staffMember");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }

            if (user != null)
            {
                return context.GetFormsByStaffMember(clientId);
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }
        [Route("GetStudentsInGroup")]
        [HttpGet]
        public List<Student> GetStudentsInGroup([FromQuery] int groupId)
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            StaffMember staffMember = HttpContext.Session.GetObject<StaffMember>("staffMember");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null && staffMember == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }
            if (context.IsGroupExist(groupId))
                return context.GetStudentsInGroup(groupId);

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;

        }
        [Route("GetSignedStudentsInForm")]
        [HttpGet]
        public string GetSignedStudentsInForm([FromQuery] int formId)
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            StaffMember staffMember = HttpContext.Session.GetObject<StaffMember>("staffMember");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null && staffMember == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }
            if (context.GetForm(formId) != null)
            {
                List<Tuple<Student, bool>> list = context.GetSignedStudentsInForm(formId);
                List<StudentSignDTO> studentSignDTOs = new List<StudentSignDTO>();
                foreach (Tuple<Student, bool> t in list)
                {
                    studentSignDTOs.Add(new StudentSignDTO(t.Item1, t.Item2));
                }
                JsonSerializerSettings options = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                };

                string json = JsonConvert.SerializeObject(studentSignDTOs, options);
                return json;
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }
        [Route("UploadExcel")]
        [HttpPost]
        public async Task<IActionResult> UploadExcel() // [FromBody] IFormFile file
        {
            StaffMember staffMember = HttpContext.Session.GetObject<StaffMember>("staffMember");
            IFormFile file = Request.Form.Files[0];
            if (staffMember != null)
            {
                if (file == null)
                {
                    return BadRequest();
                }

                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    Workbook wb = new Workbook();
                    wb.LoadFromFile(path);
                    Worksheet ws = wb.Worksheets[0];

                    //List<Account> list = new List<Account>();
                    //int row = 4;

                    //while (!string.IsNullOrEmpty(ws[row, 1].Value))
                    //{
                    //    Account a = new Account()
                    //    {
                    //        Username = ws[row, 14].Value,
                    //        Pass = ws[row, 2].Value,
                    //        Id = 0,
                    //        Email = ws[row, 14].Value,
                    //        //IsActive = true
                    //    };
                    //    list.Add(a);
                    //    row++;
                    //}
                    List<Account> list = new List<Account>();
                    int row = 4;

                    while (!string.IsNullOrEmpty(ws[row, 1].Value))
                    {
                        Account a = context.GetAccount(ws[row, 14].Value);
                        if (a == null)
                        {
                            a = new Account()
                            {
                                Username = ws[row, 2].Value,
                                Pass = "1234",
                                Id = 0,
                                Email = ws[row, 14].Value,
                                IsActiveStudent = true
                            };
                            context.AddAccount(a);
                            Student s = new Student() { Id = a.Id, Name = ws[row, 3].Value + " " + ws[row, 4].Value };
                            context.AddStudent(s);
                        }
                        else
                        {
                            Student student = context.GetStudent(a.Id);
                            if (student == null)
                            {
                                student = new Student() { Id = a.Id, Name = ws[row, 3].Value + " " + ws[row, 4].Value };
                                context.AddStudent(student);

                            }
                        }
                        list.Add(a);
                        row++;

                    }

                    Class g = context.CreateClass(staffMember.Id, list, ws[1, 1].Value);


                    //context.Entry(list[0]).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    //Class g = context.CreateGroup(user.Id, list);

                    return Ok(new { length = file.Length, name = file.FileName });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest();
                }
            }
            return Forbid();

        }
        [Route("uploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromBody] IFormFile file) // [FromBody] IFormFile file
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            if (user != null)
            {
                if (file == null)
                {
                    return BadRequest();
                }

                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    Workbook wb = new Workbook();
                    wb.LoadFromFile("test.xlsx");
                    Worksheet ws = wb.Worksheets[0];

                    List<Account> list = new List<Account>();
                    int row = 4;

                    while (!string.IsNullOrEmpty(ws[row, 1].Value))
                    {
                        Account a = context.GetAccount(ws[row, 14].Value);
                        if (a == null)
                        {
                            a = new Account()
                            {
                                Username = ws[row, 2].Value,
                                Pass = "1234",
                                Id = 0,
                                Email = ws[row, 14].Value,
                                IsActiveStudent = true
                            };
                            context.AddAccount(a);
                            Student s = new Student() { Id = a.Id, Name = ws[row, 3].Value + " " + ws[row, 4].Value };
                            context.AddStudent(s);
                        }
                        list.Add(a);
                        row++;

                    }

                    Class g = context.CreateClass(user.Id, list, ws[1, 1].Value);

                    return Ok(new { length = file.Length, name = file.FileName });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest();
                }
            }
            return Forbid();
        }
        [Route("GetClasses")]
        [HttpGet]
        public List<Class> GetClasses()
        {
            if (HttpContext.Session.GetObject<Account>("theUser") != null || HttpContext.Session.GetObject<StaffMember>("staffMember") != null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return context.GetClasses();
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }

        [Route("PostForms")]
        [HttpGet]
        public bool PostForm([FromQuery] string formJson, [FromQuery] string classesJson)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            Form form = System.Text.Json.JsonSerializer.Deserialize<Form>(formJson, options);
            List<int> classesIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(classesJson, options);
            if (form != null && classesIds.Count > 0 && HttpContext.Session.GetObject<StaffMember>("staffMember") != null)
            {
                bool worked = context.PostForm(form, classesIds);
                if (worked)
                {
                    List<Class> classes = context.GetClasses(classesIds);
                    List<int> studentIds = new List<int>();
                    foreach (Class c in classes)
                    {
                        List<Student> studentsInClass = context.GetStudentsInGroup(c.GroupId);
                        foreach (Student s in studentsInClass)
                        {
                            if (!studentIds.Contains(s.Id))
                                studentIds.Add(s.Id);
                        }
                    }
                    List<Student> students = context.GetStudents(studentIds);
                    foreach(Student s in students)
                    {
                        MailSender.SendEmail("Check It", $"{ context.GetAccount(s.Id).Email}", s.Name, "New Form", $"You Have new Form To Sign: {form.Topic}","");


                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return true;
                }
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            else
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return false;
        }

        [Route("GetFormGroups")]
        [HttpGet]
        public List<Class> GetFormGroups([FromQuery] int formId)
        {
            try
            {
                List<Class> f = context.GetFormGroups(formId);
                if(f != null && f.Count > 0)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return f;
                }
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            }
            catch
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }
        [Route("GetGroupsOfStudent")]
        [HttpGet]
        public List<Class> GetGroupsOfStudent([FromQuery] int clientId)
        {
            if (HttpContext.Session.GetObject<Account>("theUser") != null )
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return context.GetMyGroupsStudent(clientId);
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }

        [Route("GetGroupsOfStaff")]
        [HttpGet]
        public List<Class> GetGroupsOfStaff([FromQuery] int staffMemberId)
        {
            StaffMember staffMember = HttpContext.Session.GetObject<StaffMember>("staffMember");
            if (staffMember != null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                List<Class> group = context.GetMyGroupsStaff(staffMemberId);
                return group;
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }

        [Route("GetAccountsFromGroup")]
        [HttpGet]
        public List<Student> GetAccountsFromGroup([FromQuery] int Classid)
        {
            StaffMember staffMember = HttpContext.Session.GetObject<StaffMember>("staffMember");
            if (staffMember != null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return context.GetAccountsFromGroup(Classid);
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }
    }

    
}

