using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSyaEnterprise_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountRepository _UserAccountRepository;

        public UserAccountController(IUserAccountRepository userAccountRepository)
        {
            _UserAccountRepository = userAccountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GeteSyaMenulist()
        {
            var ac = await _UserAccountRepository.GeteSyaMenulist();
            return Ok(ac);
        }
    }
}