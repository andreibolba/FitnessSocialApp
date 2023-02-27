using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ServerConnectionController:BaseAPIController
    {
        [HttpGet("test")]
        public ActionResult TestConnection(){
            return Ok();
        }
    }
}