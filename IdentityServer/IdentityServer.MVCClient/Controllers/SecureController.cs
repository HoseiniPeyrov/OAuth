using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.MVCClient.Controllers
{
    [Authorize]
    public class SecureController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            ViewBag.msg = $"Identity token: {identityToken} <hr />";
            foreach (var claim in User.Claims)
            {
                ViewBag.msg += $"Claim type: {claim.Type} - Claim value: {claim.Value}<hr />";
            }




            var discoveryClient = new DiscoveryClient("https://localhost:6001");
            var metaDataResponse = await discoveryClient.GetAsync();

            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var response = await userInfoClient.GetAsync(accessToken);
            if (response.IsError)
            {
                throw new Exception("Problem accessing the UserInfo endpoint.", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(c => c.Type == "address")?.Value;
            var pass = response.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;



            return View();
        }

    }
}