using System.Web.Http.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;

namespace BasicAuthApi.Service
{
    public class BasicAuthenticationAttribute :AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;
            if (authHeader != null && authHeader.Scheme == "Basic")
            {
                var encodedCredentials = authHeader.Parameter;
                var decodedBytes = Convert.FromBase64String(encodedCredentials);
                var decodedCredentials = Encoding.UTF8.GetString(decodedBytes);
                var parts = decodedCredentials.Split(':');
                var username = parts[0];
                var password = parts.Length > 1 ? parts[1] : string.Empty;
                if (IsAuthorizedUser (username,password))
                 {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                    return;
                }
            }
            // If we reach here, the user is not authenticated
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,"Invalid Credentials");
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"MyApi\"");
        }
        private bool IsAuthorizedUser(string username, string password)
        {
            // You can validate from DB or any store
            return username == "admin" && password == "admin123";
        }
    }
}
