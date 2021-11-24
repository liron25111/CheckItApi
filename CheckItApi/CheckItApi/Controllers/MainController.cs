using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckItBL.Models;
using CheckItApi.DTO;
using CheckItApi.Services;


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
        //[Route("SignUpAccount")]
        //[HttpPost]
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

            if (user != null )
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
            if(account!= null)
            {
                EmailSender.SendEmail("Password Recovery", $"Your Password is {account.Pass} ", $"{ account.Email}", $"{ account.Username}", "CheckItDirector@gmail.com", "Check It", "CheckItApp123", "smtp.gmail.com");
                succeed = true;
            }
            return succeed;
        }
        [Route("GetSignPeople")]
        [HttpGet]
        public (int,int) GetSignPeople([FromQuery] int formId, [FromQuery] string Email)
        {
            Account user = HttpContext.Session.GetObject<Account>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return (-1,-1);
            }
            Account account = context.GetAccountByEmail(Email);

            if (user != null)
            {
                return context.GetFormSigns(formId);
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return (-1,-1);
            }
        }






    }
}
