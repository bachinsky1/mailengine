using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MailEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> accounts = new();
            accounts.Add(UserSettings.ImapUser); 
            return accounts.ToArray();
        }
          
    }
}
