using Microsoft.AspNet.Identity.EntityFramework;

namespace CloudX.DietPoints.Domain.Model
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public const int DefaultExpectedDailyDietPoints = 1500;

        public virtual int ExpectedDailyDietPoints { get; set; }
        public virtual bool Enabled { get; set; }

        public ApplicationUser()
        {
            ExpectedDailyDietPoints = DefaultExpectedDailyDietPoints;
            Enabled = true;
        }
    }
}