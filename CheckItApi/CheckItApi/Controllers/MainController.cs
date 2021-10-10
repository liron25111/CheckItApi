using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckItBL.Models;
using CheckItApi.DTO;

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
        #endregion

    }
}
