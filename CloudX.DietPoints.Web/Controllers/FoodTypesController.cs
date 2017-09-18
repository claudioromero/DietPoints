using System.Linq;
using System.Web.Http;
using CloudX.DietPoints.Web.Models;
using CloudX.DietPoints.Web.Support;
using CloudX.DietPoints.Domain;

namespace CloudX.DietPoints.Web.Controllers
{
    public class FoodTypesController : BaseApiController
    {
        [Route("api/foodTypes")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult List()
        {
            var types = Context.FoodTypes.Select(x => new FoodTypeViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return Ok(types);
        }
    }
}