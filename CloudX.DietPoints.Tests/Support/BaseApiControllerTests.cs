using CloudX.DietPoints.Domain;
using CloudX.DietPoints.Domain.Model;
using CloudX.DietPoints.Web.Support;
using Moq;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using CloudX.DietPoints.Web;
using CloudX.DietPoints.Domain.Security;
using System.Net.Http;
using System;
using Microsoft.Owin;
using System.Linq;

namespace CloudX.DietPoints.Tests.Support
{
    public class BaseApiControllerTests<TController> where TController : BaseApiController, new()
    {
        private IOwinContext owinContext;
        protected ModelDbContext Context { get; private set; }
        protected TController Controller { get; private set; }
        protected int DessertId { get; private set; }
        protected int SteakId { get; private set; }
        protected int ChickenId { get; private set; }
        protected int SaladId { get; private set; }
        protected int CheeseId { get; private set; }
        protected string LoggedUserId { get; private set; }

        public virtual void TestInitialize()
        {
            Context = ModelDbContext.Create();

            CreateTestData();
            CreateController();
        }

        public virtual void TestCleanup()
        {
            Context.Dispose();
            owinContext.Set<ModelDbContext>(null);
        }

        private void CreateController()
        { 
            Controller = new TController()
            {
                User = MockUser()
            };

            Controller.Request = new HttpRequestMessage(HttpMethod.Post,
                new Uri("test", UriKind.Relative)
                );

            owinContext = new OwinContext();
            Controller.Request.SetOwinContext(owinContext);
            owinContext.Set(Context);
        }

        private void CreateTestData()
        {
            ClearData();

            CreateUsers();

            var dessert = Context.FoodTypes.Add(new FoodType
            {
                CaloriesMultiplier = 1.5,
                Name = "Dessert",
                FoodGroup = FoodGroup.Dairy,
                IsLowFat = true,
                SaturatedFatPercent = 4
            });

            var steak = Context.FoodTypes.Add(new FoodType
            {
                CaloriesMultiplier = 1.8,
                Name = "Steak",
                FoodGroup = FoodGroup.Meat,
                IsLowFat = false,
                SaturatedFatPercent = 73
            });

            var chicken = Context.FoodTypes.Add(new FoodType
            {
                CaloriesMultiplier = 1.8,
                Name = "Chicken",
                FoodGroup = FoodGroup.Meat,
                IsLowFat = false,
                SaturatedFatPercent = 48
            });

            var salad = Context.FoodTypes.Add(new FoodType
            {
                CaloriesMultiplier = 0.5,
                Name = "Salad",
                FoodGroup = FoodGroup.Vegetables,
                IsLowFat = true,
                SaturatedFatPercent = 15
            });

            var cheese = Context.FoodTypes.Add(new FoodType
            {
                CaloriesMultiplier = 1.2,
                Name = "Cheese",
                FoodGroup = FoodGroup.Dairy,
                IsLowFat = true,
                SaturatedFatPercent = 17
            });

            // Define the rules applicable to diet points
            DefineRules(Context);

            Context.SaveChanges();

            DessertId = dessert.Id;
            SteakId = steak.Id;
            ChickenId = chicken.Id;
            SaladId = salad.Id;
            CheeseId = cheese.Id;
        }

        /// <summary>
        /// Defines the test rules for diet points
        /// </summary>
        /// <param name="context">Specifies the context</param>
        private void DefineRules(ModelDbContext context)
        {
            SaveRule(context, new DietPointRule
            {
                Id = 1,
                FoodGroup = FoodGroup.Meat,
                MinFatLevel = 50,
                MaxFatLevel = 100,
                FixedCalories = 0,
                IsLowFat = false,
                Multiplier = 1.2,
                Bias = 0
            });

            SaveRule(context, new DietPointRule
            {
                Id = 2,
                FoodGroup = FoodGroup.Meat,
                MinFatLevel = 25,
                MaxFatLevel = 50,
                FixedCalories = 0,
                IsLowFat = false,
                Multiplier = 1.1,
                Bias = 0
            });

            SaveRule(context, new DietPointRule
            {
                Id = 3,
                FoodGroup = FoodGroup.Dairy,
                MinFatLevel = 0,
                MaxFatLevel = 0,
                FixedCalories = 0,
                IsLowFat = true,
                Multiplier = 0.8,
                Bias = 0
            });

            SaveRule(context, new DietPointRule
            {
                Id = 4,
                FoodGroup = FoodGroup.Vegetables,
                MinFatLevel = 0,
                MaxFatLevel = 0,
                FixedCalories = 1000,
                IsLowFat = true,
                Multiplier = 0,
                Bias = 947
            });
        }

        private void SaveRule(ModelDbContext context, DietPointRule rule)
        {
            if (!context.DietPointRules.Any(x => x.Id == rule.Id))
            {
                context.DietPointRules.Add(rule);
            }
        }

        private void CreateUsers()
        {
            var options = new IdentityFactoryOptions<ApplicationUserManager>();

            var userManager = ApplicationUserManagerConfigurator.Create(options, Context);

            var r = userManager.Register("ivo@test.com", "Password").Result;

            LoggedUserId = r.User.Id;

            r = userManager.Register("bowie@test.com", "bowiebowie").Result;
        }

        private void ClearData()
        {
            Context.Database.ExecuteSqlCommand("DELETE FROM Entries");
            Context.Database.ExecuteSqlCommand("DELETE FROM FoodTypes");
            Context.Database.ExecuteSqlCommand("DELETE FROM DietPointRules");
            Context.Database.ExecuteSqlCommand("DELETE FROM AspNetUsers");
        }

        private IPrincipal MockUser()
        {
            var userMock = new Mock<IPrincipal>();
            var identityMock = new Mock<ClaimsIdentity>();
            identityMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim("test", LoggedUserId));
            userMock.Setup(x => x.Identity).Returns(identityMock.Object);

            return userMock.Object;
        }
    }
}