using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Integration.Tests.Extensions
{
    public static class ApiControllerExtension
    {
        public static void MockApiCurrentUser(this ApiController apiController, string userName, string userId)
        {
            var identity = new GenericIdentity(userName);

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userName));

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));

            var principal = new GenericPrincipal(identity, null);
            apiController.User = principal;
        }
    }
}
