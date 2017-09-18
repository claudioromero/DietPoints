using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using CloudX.DietPoints.Web.Models;
using CloudX.DietPoints.Web.Support;
using CloudX.DietPoints.Domain;

namespace CloudX.DietPoints.Web.Controllers
{
    [Authorize]
    public class DietPointsController : BaseApiController
    {
        [Route("api/me/dietpoints")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var userId = User.Identity.GetUserId();

            var user = Context.Users.Where(x => x.Id == userId).SingleOrDefault();

            var model = new DietPointsViewModel
            {
                ExpectedDailyDietPoints = user.ExpectedDailyDietPoints,
                TodaysDietPoints = Context.Entries.Where(x => x.User.Id == userId && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(DateTime.Today))
                                                  .Select(x => x.DietPoints)
                                                  .DefaultIfEmpty(0)
                                                  .Sum()
            };

            return Ok(model);
        }

        [Route("api/me/dietpoints/configuration")]
        [HttpPut]
        public IHttpActionResult ConfigureDietPoints(ConfigureDietPointsViewModel model)
        {
            var userId = User.Identity.GetUserId();

            var profile = Context.Users.SingleOrDefault(x => x.Id == userId);

            if (profile == null)
                return NotFound();

            profile.ExpectedDailyDietPoints = model.ExpectedDailyDietPoints;

            Context.SaveChanges();

            return NoContent();
        }
    }
}