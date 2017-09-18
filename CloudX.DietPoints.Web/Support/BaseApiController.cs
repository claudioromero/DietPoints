using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Linq;
using CloudX.DietPoints.Domain;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;

namespace CloudX.DietPoints.Web.Support
{
    [ValidateModel]
    public abstract class BaseApiController : ApiController
    {
        protected ModelDbContext Context => Request.GetOwinContext().Get<ModelDbContext>();

        protected IHttpActionResult Created(string location)
        {
            return Created<object>(location, null);
        }

        protected IHttpActionResult NoContent()
        {
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected IHttpActionResult BadRequest(IEnumerable<string> errors)
        {
            var formattedErrors = errors.Select(x => x.Last() != '.' ? x + "." : x); //adds a period (.) at the end of the error message
            return BadRequest(string.Join(" ", errors));
        }
    }
}