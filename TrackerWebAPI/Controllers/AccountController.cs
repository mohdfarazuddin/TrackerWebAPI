using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace TrackerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        [Route("external-login")]
        public IActionResult ExternalLogin(string provider = "Google")
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("") };
            if (provider == "Google")
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

        [HttpPost("auth/google")]
        public async Task<IActionResult> GoogleLogin(string idtoken)
        {
            if (idtoken == null)
                throw new Exception();
            Payload payload;
            try
            {
                payload = await ValidateAsync(idtoken, new ValidationSettings
                {
                    Audience = new[] { "829498266017-32fjff71n24bn68rvgg0eabhigk4mpnl.apps.googleusercontent.com" }
                });
                // It is important to add your ClientId as an audience in order to make sure
                // that the token is for your application!
                await Login();
                return Ok();
            }
            catch
            {
                // Invalid token
                return BadRequest();
            }

        }

        public async Task<IActionResult> Login()
        {

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("Admin","user"),
             }, "Cookies");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
