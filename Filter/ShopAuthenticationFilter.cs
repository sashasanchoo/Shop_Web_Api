using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace IShop.Filter
{
    public class ShopAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get 
            { 
                return false;
            }
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            context.Principal = null;
            AuthenticationHeaderValue authentication = context.Request.Headers.Authorization;
            if(authentication != null && authentication.Scheme == "Basic")
            {
                var authData = Encoding.ASCII.GetString(Convert.FromBase64String(authentication.Parameter)).Split(':');
                var roles = new string[] { "user" };
                var login = authData[0];
                context.Principal = new GenericPrincipal(new GenericIdentity(login), roles);
                Console.WriteLine("Works");
            }
            else
            {
                Console.WriteLine("Doesn't work");
            }
            if(context.Principal == null)
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[]
                {
                    new AuthenticationHeaderValue("Basic")
                }, context.Request);
            }
            Console.WriteLine("Before return");
            return Task.FromResult<object>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }
}
