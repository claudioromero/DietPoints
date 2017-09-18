using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CloudX.DietPoints.Tests.Support;
using CloudX.DietPoints.Web.Models;
using CloudX.DietPoints.Domain.Model;
using System;
using CloudX.DietPoints.Web.Controllers;
using System.Web.Http.Results;
using CloudX.DietPoints.Domain;
using System.Linq;

namespace CloudX.DietPoints.Tests
{
    [TestClass]
    public class DietPointsControllerTests : BaseApiControllerTests<DietPointsController>
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        public void ConfigureDietPointsAndGetExpectedDailyDietPoints()
        {
            var model = new ConfigureDietPointsViewModel
            {
                ExpectedDailyDietPoints = 1200
            };

            var put = Controller.ConfigureDietPoints(model) as StatusCodeResult;

            Assert.IsNotNull(put);
            Assert.AreEqual(HttpStatusCode.NoContent, put.StatusCode);

            var get = Controller.Get() as OkNegotiatedContentResult<DietPointsViewModel>;

            Assert.IsNotNull(get);
            Assert.AreEqual(1200, get.Content.ExpectedDailyDietPoints);

            model.ExpectedDailyDietPoints = 1000;

            put = Controller.ConfigureDietPoints(model) as StatusCodeResult;

            Assert.IsNotNull(put);
            Assert.AreEqual(HttpStatusCode.NoContent, put.StatusCode);

            get = Controller.Get() as OkNegotiatedContentResult<DietPointsViewModel>;

            Assert.IsNotNull(get);
            Assert.AreEqual(1000, get.Content.ExpectedDailyDietPoints);
        }

        private void SaveEntry(Entry entry, int foodTypeId)
        {
            entry.User = Context.Users.Single(x => x.Id == LoggedUserId);
            entry.FoodType = Context.FoodTypes.Single(x => x.Id == foodTypeId);
            entry.CalculateDietPoints(Context);

            Context.Entries.Add(entry);
            Context.SaveChanges();
        }

        [TestMethod]
        public void TodaysDietPoints()
        {
            SaveEntry(new Entry
                      {
                          Date = DateTime.Now,
                          Meal = "Asado",
                          Calories = 1500
                      }, 
                      SteakId);

            SaveEntry(new Entry
                      {
                          Date = DateTime.Now,
                          Meal = "Tiramisu",
                          Calories = 700
                      }, 
                      DessertId);

            SaveEntry(new Entry
                      {
                          Date = DateTime.Now.AddDays(-1),
                          Meal = "Tomato Salad",
                          Calories = 300
                      }, 
                      SaladId);

            var get = Controller.Get() as OkNegotiatedContentResult<DietPointsViewModel>;
            
            Assert.IsNotNull(get);

            Assert.AreEqual(2360, get.Content.TodaysDietPoints);
        }
    }
}