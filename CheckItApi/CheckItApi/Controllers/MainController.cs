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
        #endregion

    }
}
