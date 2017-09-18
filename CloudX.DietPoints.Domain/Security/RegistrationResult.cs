using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudX.DietPoints.Domain.Model;

namespace CloudX.DietPoints.Domain.Security
{
    public class RegistrationResult : IdentityResult
    {
        private RegistrationResult() : base(true)
        {
        }

        private RegistrationResult(IEnumerable<string> errors) : base(errors)
        {
        }

        public ApplicationUser User { get; private set; }

        public static RegistrationResult Create(IdentityResult result, ApplicationUser user)
        {
            var registrationResult = result.Succeeded
                ? new RegistrationResult()
                : new RegistrationResult(result.Errors);

            registrationResult.User = user;
            return registrationResult;
        }
    }
}
