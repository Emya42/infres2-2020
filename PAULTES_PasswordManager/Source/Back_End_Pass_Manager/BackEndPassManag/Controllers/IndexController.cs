using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BackEndPassManag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        // GET: api/Index
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Bienvenue sur l'API REST !" };
        }
    }
}
