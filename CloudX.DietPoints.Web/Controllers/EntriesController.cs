using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using CloudX.DietPoints.Domain.Model;
using CloudX.DietPoints.Web.Models;
using CloudX.DietPoints.Web.Support;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using System;

namespace CloudX.DietPoints.Web.Controllers
{
    public class EntriesController : BaseApiController
    {
        [Route("api/me/entries/{Id}", Name = "GetEntryById")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var userId = User.Identity.GetUserId();
            var entry = Context.Entries.Where(x => x.User.Id == userId && x.Id == id)
                .Select(x => new EntryViewModel
                {
                    Id = x.Id,
                    Date = x.Date,
                    Meal = x.Meal,
                    Calories = x.Calories,
                    DietPoints = x.DietPoints,
                    FoodType = new FoodTypeViewModel { Id = x.FoodType.Id, Name = x.FoodType.Name }
                })
                .SingleOrDefault();

            if (entry == null)
                return NotFound();

            return Ok(entry);
        }

        [Route("api/me/export")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage Export()
        {
            const string separator = ",";

            var sb = new StringBuilder();
            sb.Append("Date" + separator + "Meal" + separator + "Type" + separator + "Calories" + separator + "Diet Points");
            sb.Append(Environment.NewLine);

            var userId = User.Identity.GetUserId();
            IQueryable<Entry> entries = Context.Entries.Where(x => x.User.Id == userId).OrderByDescending(x => x.Date);

            foreach (var entry in entries)
            {
                sb.Append(entry.Date.ToString() + separator);
                sb.Append(entry.Meal + separator);
                sb.Append(entry.FoodType.Name + separator);
                sb.Append(entry.Calories.ToString() + separator);
                sb.Append(entry.DietPoints.ToString());
                sb.Append(Environment.NewLine);
            }

            byte[] rawBytes = Encoding.UTF8.GetBytes(sb.ToString());

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new MemoryStream(rawBytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "DietPoints.csv"
            };

            return result;
        }

        [Route("api/me/entries")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult List([FromUri] GetEntriesViewModel model)
        {
            const int pageSize = 10;
            var userId = User.Identity.GetUserId();

            IQueryable<Entry> entries = Context.Entries.Where(x => x.User.Id == userId).OrderByDescending(x => x.Date);

            if (model == null)
                model = new GetEntriesViewModel();

            if (model.DateFrom.HasValue && model.DateTo.HasValue)
            {
                entries = entries.Where(x => x.Date >= model.DateFrom.Value && x.Date <= model.DateTo.Value);
            }

            if (model.TimeFrom.HasValue && model.TimeTo.HasValue)
            {
                var from = model.TimeFrom.Value.TimeOfDay;
                var to = model.TimeTo.Value.TimeOfDay;
                entries = from x in entries
                          let time = DbFunctions.CreateTime(x.Date.Hour, x.Date.Minute, x.Date.Second)
                          where time >= @from && time <= to
                          select x;
            }

            var page = new PageViewModel<EntryViewModel>
            {
                TotalRecords = entries.Count(),
                Items = new List<EntryViewModel>()
            };

            var pageIndex = model.Page < 1 ? 0 : model.Page - 1;

            var pagedResults = entries.Skip(pageIndex * pageSize).Take(pageSize);

            foreach (var entry in pagedResults)
            {
                var entryViewModel = new EntryViewModel
                {
                    Id = entry.Id,
                    Date = entry.Date,
                    Meal = entry.Meal,
                    Calories = entry.Calories,
                    DietPoints = entry.DietPoints,
                    FoodType = new FoodTypeViewModel { Id = entry.FoodType.Id, Name = entry.FoodType.Name }
                };

                page.Items.Add(entryViewModel);
            }

            return Ok(page);
        }

        [Route("api/me/entries")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Post(EntryViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = Context.Users.SingleOrDefault(x => x.Id == userId);
            var foodType = Context.FoodTypes.Single(x => x.Id == model.FoodType.Id);

            var entry = new Entry
            {
                Date = model.Date,
                Meal = model.Meal,
                Calories = model.Calories.Value,
                User = user,
                FoodType = foodType
            };

            entry.CalculateDietPoints(Context);

            Context.Entries.Add(entry);
            Context.SaveChanges();

            return Created(Url.Link("GetEntryById", new CreatedViewModel { Id = entry.Id }));
        }

        [Route("api/me/entries/{Id}")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult Put(int id, EntryViewModel model)
        {
            var userId = User.Identity.GetUserId();

            var entry = Context.Entries.SingleOrDefault(x => x.Id == id && x.User.Id == userId);

            if (entry == null)
                return NotFound();

            var foodType = Context.FoodTypes.Single(x => x.Id == model.FoodType.Id);

            entry.Date = model.Date;
            entry.Meal = model.Meal;
            entry.Calories = model.Calories.Value;
            entry.FoodType = foodType;
            entry.CalculateDietPoints(Context);

            Context.SaveChanges();

            return NoContent();
        }

        [Route("api/me/entries/{Id}")]
        [Authorize]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();

            var entry = Context.Entries.SingleOrDefault(x => x.Id == id && x.User.Id == userId);

            if (entry == null)
                return NotFound();

            Context.Entries.Remove(entry);

            Context.SaveChanges();

            return NoContent();
        }
    }
}