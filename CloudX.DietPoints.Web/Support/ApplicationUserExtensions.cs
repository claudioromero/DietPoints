using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using CloudX.DietPoints.Domain.Model;

namespace CloudX.DietPoints.Web.Support
{
    public static class ApplicationUserExtensions
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this ApplicationUser self,
            UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(self, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}