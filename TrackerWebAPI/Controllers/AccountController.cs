using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrackerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        [Route("external-login")]
        public IActionResult GoogleLogin(string provider = "Google")
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("") };
            if(provider == "Google")
                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            else
                return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        //[HttpGet]
        //[Route("facebook-login")]
        //public IActionResult FacebookLogin()
        //{
        //    var properties = new AuthenticationProperties { RedirectUri = Url.Action("login-response") };
        //    return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        //}

        [HttpGet]
        [Route("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        //[HttpGet]
        //[Route("login-response")]
        //public async Task<IActionResult> LoginResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    var claims = result.Principal.Identities.FirstOrDefault().Claims
        //                                                .Select(claims => new
        //                                                {
        //                                                    claims.Issuer,
        //                                                    claims.OriginalIssuer,
        //                                                    claims.Type,
        //                                                    claims.Value
        //                                                });
        //    return Ok(claims);
        //}

    }
}
