using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Web;

namespace GigHub.Integration.Tests.Extensions
{
    public static class ControllerExtensions
    {
        public static void MockCurrentUser(this Controller controller, string userName, string userId)
        {
            var identity = new GenericIdentity(userName);

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userName));

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));

            var principal = new GenericPrincipal(identity, null);
            // Not a best way
            //var mockHttpContext = new Mock<HttpContextBase>();
            //mockHttpContext.SetupGet(c => c.User).Returns(principal);

            //var mockControllerContext = new Mock<ControllerContext>();
            //mockControllerContext.SetupGet(c => c.HttpContext).Returns(mockHttpContext.Object);    

            //controller.ControllerContext = mockControllerContext.Object;        

            //Cleaner way
            controller.ControllerContext = Mock.Of<ControllerContext>(
                ctx => ctx.HttpContext == Mock.Of<HttpContextBase>(
                    http => http.User == principal));
        }
    }
}
