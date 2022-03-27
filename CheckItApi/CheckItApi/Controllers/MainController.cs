using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckItBL.Models;
using CheckItApi.DTO;
using CheckItApi.Services;
using System.IO;
using Spire.Xls;
using System.Text;

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
                EmailSender.SendEmail("Your Password Changed", $"Your New Password is {account.Pass} ", $"{ account.Email}", $"{ account.Username}", "CheckItDirector@gmail.com", "Check It", "CheckItApp123", "smtp.gmail.com");

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
            if (account != null)
            {
                EmailSender.SendEmail("Password Recovery", $"Your Password is {account.Pass} ", $"{ account.Email}", $"{ account.Username}", "CheckItDirector@gmail.com", "Check It", "CheckItApp123", "smtp.gmail.com");
                succeed = true;
            }
            return succeed;
        }
        [Route("Signs")]
        [HttpGet]
        public int Signs([FromQuery] int formId)
        {
            if(context.GetForm(formId) == null)
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
                return context.IsSigned(Email,formId);
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }
        }
        [Route("GetForms")]
        [HttpGet]
        public List<Form> GetForms([FromQuery] int clientId)
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }

            if (user != null)
            {
                return context.GetFormsByAccount(clientId);
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        [Route("UploadExcel")]
        [HttpPost]
        public async Task<IActionResult> UploadExcel() // [FromBody] IFormFile file
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            IFormFile file = Request.Form.Files[0];
            if (user != null || user == null)
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
                            if(student == null)
                            {
                                student = new Student() { Id = a.Id, Name = ws[row, 3].Value + " " + ws[row, 4].Value };
                                context.AddStudent(student);

                            }
                        }
                        list.Add(a);
                        row++;

                    }

                    Class g = context.CreateClass(1001, list, ws[1, 1].Value);


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
        public async Task<IActionResult> UploadFile([FromBody]  IFormFile file) // [FromBody] IFormFile file
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

                        Class g = context.CreateClass(user.Id, list,ws[1,1].Value);

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
            if (true || HttpContext.Session.GetObject<Account>("theUser") != null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return context.GetClasses();
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }

    }

}

