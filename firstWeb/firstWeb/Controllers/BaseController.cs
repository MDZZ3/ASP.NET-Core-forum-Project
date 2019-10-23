using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace firstWeb.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;
        

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    }
}