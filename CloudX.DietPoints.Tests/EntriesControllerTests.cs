using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CloudX.DietPoints.Tests.Support;
using CloudX.DietPoints.Web.Models;
using CloudX.DietPoints.Web.Controllers;
using Moq;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using CloudX.DietPoints.Web.Support;

namespace CloudX.DietPoints.Tests
{
    [TestClass]
    public class EntriesControllerTests : BaseApiControllerTests<EntriesController>
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns((string _, object x) => ((CreatedViewModel)x).Id.ToString());
            Controller.Url = urlHelperMock.Object;
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }

        /// <summary>
        /// This test ensures that the application is capable of dealing with the "Oojed coefficient"
        /// </summary>
        [TestMethod]
        public void OojedTest()
        {
            // -----------------------------------------------------------------------------------------------------------------------
            // Scenario #1: Watch it pass.  
            // The Oojed coefficient is applicable, because the number of calores is greater than 1,000
            // -----------------------------------------------------------------------------------------------------------------------
            int oOjedValue = 947;
            int calories = 1500;

            var now = DateTime.Now;

            var created = Controller.Post(new EntryViewModel
            {
                Date = now,
                Meal = "Caesar Salad",
                Calories = calories,
                FoodType = new FoodTypeViewModel { Id = SaladId }
            }) as CreatedNegotiatedContentResult<object>;

            Assert.IsNotNull(created);

            //Get Id from mocked location
            var id = int.Parse(created.Location.ToString());

            var get = Controller.Get(id) as OkNegotiatedContentResult<EntryViewModel>;

            Assert.IsNotNull(get);

            Assert.AreEqual(now.Date, get.Content.Date.Date);
            Assert.AreEqual(now.Hour, get.Content.Date.Hour);
            Assert.AreEqual(now.Minute, get.Content.Date.Minute);

            Assert.AreEqual("Caesar Salad", get.Content.Meal);
            Assert.AreEqual(calories, get.Content.Calories);
            Assert.AreEqual(oOjedValue, get.Content.DietPoints);


            // -----------------------------------------------------------------------------------------------------------------------
            // Scenario #2: 
            // The Oojed coefficient is not applicable, because the number of calores is lower than or equal to 1,000
            // -----------------------------------------------------------------------------------------------------------------------
            calories = 1000;
            created = Controller.Post(new EntryViewModel
            {
                Date = now,
                Meal = "Yet another Salad",
                Calories = calories,
                FoodType = new FoodTypeViewModel { Id = SaladId }
            }) as CreatedNegotiatedContentResult<object>;

            Assert.IsNotNull(created);

            //Get Id from mocked location
            id = int.Parse(created.Location.ToString());

            get = Controller.Get(id) as OkNegotiatedContentResult<EntryViewModel>;

            Assert.IsNotNull(get);

            Assert.AreEqual(now.Date, get.Content.Date.Date);
            Assert.AreEqual(now.Hour, get.Content.Date.Hour);
            Assert.AreEqual(now.Minute, get.Content.Date.Minute);

            Assert.AreEqual("Yet another Salad", get.Content.Meal);
            Assert.AreEqual(calories, get.Content.Calories);
            Assert.AreEqual(calories * 0.5, get.Content.DietPoints);
        }


        /// <summary>
        /// This test ensures that the application is capable of dealing with saturated fat ranges.
        /// </summary>
        [TestMethod]
        public void SaturatedFatTests()
        {
            // -----------------------------------------------------------------------------------------------------------
            // Scenario #1
            // -----------------------------------------------------------------------------------------------------------
            // Los tipos de comida que son Carnes deben guardar, además del multiplicador, el % de grasa.
            // Si el % de grasa es mayor al 50 %, los diet points se multiplican por 1.2.
            int calories = 1000;
            double applicableMultiplier = 1.2;

            var now = DateTime.Now;

            var created = Controller.Post(new EntryViewModel
            {
                Date = now,
                Meal = "Rib Eye",
                Calories = calories,
                FoodType = new FoodTypeViewModel { Id = SteakId }
            }) as CreatedNegotiatedContentResult<object>;

            Assert.IsNotNull(created);

            var id = int.Parse(created.Location.ToString());
            var get = Controller.Get(id) as OkNegotiatedContentResult<EntryViewModel>;

            Assert.IsNotNull(get);
            Assert.AreEqual(now.Date, get.Content.Date.Date);
            Assert.AreEqual(now.Hour, get.Content.Date.Hour);
            Assert.AreEqual(now.Minute, get.Content.Date.Minute);

            Assert.AreEqual("Rib Eye", get.Content.Meal);
            Assert.AreEqual(calories, get.Content.Calories);
            Assert.AreEqual(calories * applicableMultiplier, get.Content.DietPoints);


            // -----------------------------------------------------------------------------------------------------------
            // Scenario #2:
            // -----------------------------------------------------------------------------------------------------------
            // Si % de grasa es entre 25% y 50%, los diet points se multiplican por 1.1
            applicableMultiplier = 1.1;
            now = DateTime.Now;

            created = Controller.Post(new EntryViewModel
            {
                Date = now,
                Meal = "Chicken Breast",
                Calories = calories,
                FoodType = new FoodTypeViewModel { Id = ChickenId }
            }) as CreatedNegotiatedContentResult<object>;

            Assert.IsNotNull(created);

            id = int.Parse(created.Location.ToString());
            get = Controller.Get(id) as OkNegotiatedContentResult<EntryViewModel>;

            Assert.IsNotNull(get);
            Assert.AreEqual(now.Date, get.Content.Date.Date);
            Assert.AreEqual(now.Hour, get.Content.Date.Hour);
            Assert.AreEqual(now.Minute, get.Content.Date.Minute);

            Assert.AreEqual("Chicken Breast", get.Content.Meal);
            Assert.AreEqual(calories, get.Content.Calories);
            Assert.AreEqual(calories * applicableMultiplier, get.Content.DietPoints);


            // -----------------------------------------------------------------------------------------------------------
            // Scenario #3:
            // -----------------------------------------------------------------------------------------------------------
            // Los tipos de comida que son Lácteos deben guardar, además del multiplicador, si son bajos en grasas o no. 
            // Si lo son, los diet points deben multiplicarse por 0.8
            calories = 1500;
            applicableMultiplier = 0.8;
            now = DateTime.Now;

            created = Controller.Post(new EntryViewModel
            {
                Date = now,
                Meal = "Cheese",
                Calories = calories,
                FoodType = new FoodTypeViewModel { Id = CheeseId }
            }) as CreatedNegotiatedContentResult<object>;

            Assert.IsNotNull(created);

            id = int.Parse(created.Location.ToString());
            get = Controller.Get(id) as OkNegotiatedContentResult<EntryViewModel>;

            Assert.IsNotNull(get);
            Assert.AreEqual(now.Date, get.Content.Date.Date);
            Assert.AreEqual(now.Hour, get.Content.Date.Hour);
            Assert.AreEqual(now.Minute, get.Content.Date.Minute);

            Assert.AreEqual("Cheese", get.Content.Meal);
            Assert.AreEqual(calories, get.Content.Calories);
            Assert.AreEqual(calories * applicableMultiplier, get.Content.DietPoints);
        }


        [TestMethod]
        public void PostGetPutDeleteEntry()
        {
            var now = DateTime.Now;

            var created = Controller.Post(new EntryViewModel
            {
                Date = now,
                Meal = "Asado",
                Calories = 1500,
                FoodType = new FoodTypeViewModel { Id = SteakId }
            }) as CreatedNegotiatedContentResult<object>;

            Assert.IsNotNull(created);

            //Get Id from mocked location
            var id = int.Parse(created.Location.ToString());

            var get = Controller.Get(id) as OkNegotiatedContentResult<EntryViewModel>;

            Assert.IsNotNull(get);

            Assert.AreEqual(now.Date, get.Content.Date.Date);
            Assert.AreEqual(now.Hour, get.Content.Date.Hour);
            Assert.AreEqual(now.Minute, get.Content.Date.Minute);

            Assert.AreEqual("Asado", get.Content.Meal);
            Assert.AreEqual(1500, get.Content.Calories);
            Assert.AreEqual(1800, get.Content.DietPoints);

            var yesterday = now.AddDays(-1);

            var put = Controller.Put(id, new EntryViewModel
            {
                Date = yesterday,
                Meal = "Tomato Salad",
                Calories = 120,
                FoodType = new FoodTypeViewModel { Id = SaladId }
            }) as StatusCodeResult;

            Assert.IsNotNull(put);
            Assert.AreEqual(HttpStatusCode.NoContent, put.StatusCode);

            get = Controller.Get(id) as OkNegotiatedContentResult<EntryViewModel>;

            Assert.IsNotNull(get);

            Assert.AreEqual(yesterday.Date, get.Content.Date.Date);
            Assert.AreEqual(yesterday.Hour, get.Content.Date.Hour);
            Assert.AreEqual(yesterday.Minute, get.Content.Date.Minute);

            Assert.AreEqual("Tomato Salad", get.Content.Meal);
            Assert.AreEqual(120, get.Content.Calories);
            Assert.AreEqual(60, get.Content.DietPoints);

            var delete = Controller.Delete(id) as StatusCodeResult;
            Assert.IsNotNull(delete);
            Assert.AreEqual(HttpStatusCode.NoContent, delete.StatusCode);

            var notFound = Controller.Get(id) as NotFoundResult;

            Assert.IsNotNull(notFound);
        }

        private void CreateEntries()
        {
            for (var i = 2; i < 27; i++)
            {
                var hour = i%2 == 0 ? 5 : 17;

                var post = Controller.Post(new EntryViewModel
                {
                    Date = new DateTime(2016, 1, i, hour, 0, 0),
                    Meal = "Asado " + i,
                    Calories = 1500 + i,
                    FoodType = new FoodTypeViewModel { Id = SteakId }
                }) as CreatedNegotiatedContentResult<object>;

                Assert.IsNotNull(post);
            }
        }

        [TestMethod]
        public void ListEntries()
        {
            CreateEntries();

            var list = Controller.List(new GetEntriesViewModel()) as OkNegotiatedContentResult<PageViewModel<EntryViewModel>>;

            Assert.IsNotNull(list);

            Assert.AreEqual(10, list.Content.Items.Count);
            Assert.AreEqual(25, list.Content.TotalRecords);

            //Assert over one item just to check if values are being returned and the sorting is alright

            var entry = list.Content.Items[1];
            Assert.AreEqual(25, entry.Date.Day);
            Assert.AreEqual(1, entry.Date.Month);
            Assert.AreEqual(2016, entry.Date.Year);
            Assert.AreEqual(17, entry.Date.Hour);
            Assert.AreEqual(0, entry.Date.Minute);

            Assert.AreEqual("Asado 25", entry.Meal);
            Assert.AreEqual(1525, entry.Calories);
            Assert.AreEqual(1830, entry.DietPoints);
        }

        [TestMethod]
        public void PaginateEntries()
        {
            CreateEntries();

            var list = Controller.List(new GetEntriesViewModel { Page = 3 }) as OkNegotiatedContentResult<PageViewModel<EntryViewModel>>;

            Assert.IsNotNull(list);

            Assert.AreEqual(5, list.Content.Items.Count);
            Assert.AreEqual(25, list.Content.TotalRecords);

            //Assert over one item just to check if values are being returned and the sorting is alright

            var entry = list.Content.Items[1];
            Assert.AreEqual(5, entry.Date.Day);
            Assert.AreEqual(1, entry.Date.Month);
            Assert.AreEqual(2016, entry.Date.Year);
            Assert.AreEqual(17, entry.Date.Hour);
            Assert.AreEqual(0, entry.Date.Minute);

            Assert.AreEqual("Asado 5", entry.Meal);
            Assert.AreEqual(1505, entry.Calories);
        }

        [TestMethod]
        public void FilterEntriesByDate()
        {
            CreateEntries();

            var from = new DateTime(2016, 1, 5, 0, 0, 0);
            var to = new DateTime(2016, 1, 7, 23, 59, 59);

            var list = Controller.List(new GetEntriesViewModel { DateFrom = from, DateTo = to }) as OkNegotiatedContentResult<PageViewModel<EntryViewModel>>;

            Assert.IsNotNull(list);

            Assert.AreEqual(3, list.Content.Items.Count);
            Assert.AreEqual(3, list.Content.TotalRecords);

            //Assert over one item just to check if values are being returned and the sorting is alright

            var entry = list.Content.Items[1];
            Assert.AreEqual(6, entry.Date.Day);
            Assert.AreEqual(1, entry.Date.Month);
            Assert.AreEqual(2016, entry.Date.Year);
            Assert.AreEqual(5, entry.Date.Hour);
            Assert.AreEqual(0, entry.Date.Minute);

            Assert.AreEqual("Asado 6", entry.Meal);
            Assert.AreEqual(1506, entry.Calories);
        }

        [TestMethod]
        public void FilterEntriesByTime()
        {
            CreateEntries();

            var from = new DateTime(2016, 1, 1, 12, 0, 0);
            var to = new DateTime(2016, 1, 1, 18, 0, 0);

            var list = Controller.List(new GetEntriesViewModel { TimeFrom = from, TimeTo = to }) as OkNegotiatedContentResult<PageViewModel<EntryViewModel>>;

            Assert.IsNotNull(list);

            Assert.AreEqual(10, list.Content.Items.Count);
            Assert.AreEqual(12, list.Content.TotalRecords);

            //Assert over one item just to check if values are being returned and the sorting is alright

            var entry = list.Content.Items[1];
            Assert.AreEqual(23, entry.Date.Day);
            Assert.AreEqual(1, entry.Date.Month);
            Assert.AreEqual(2016, entry.Date.Year);
            Assert.AreEqual(17, entry.Date.Hour);
            Assert.AreEqual(0, entry.Date.Minute);

            Assert.AreEqual("Asado 23", entry.Meal);
            Assert.AreEqual(1523, entry.Calories);
        }
    }
}