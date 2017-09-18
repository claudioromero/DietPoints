using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using CloudX.DietPoints.Domain;
using CloudX.DietPoints.Domain.Model;
using CloudX.DietPoints.Web.Support;
using CloudX.DietPoints.Domain.Security;

namespace CloudX.DietPoints.Web
{
    // Configure the application user manager which is used in this application.
    public class ApplicationUserManagerConfigurator
    {
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var dbContext = context.Get<ModelDbContext>();
            return Create(options, dbContext);
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, ModelDbContext dbContext)
        {

            var manager = ApplicationUserManager.Create(dbContext);

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider == null)
                dataProtectionProvider = new DpapiDataProtectionProvider();

            manager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));

            return manager;
        }
    }


    // Configure the application sign-in manager which is used in this application.  
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            :
                base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager) UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}