using System;
using System.Data.Entity.Migrations;
using CloudX.DietPoints.Domain.Model;
using System.Linq;

namespace CloudX.DietPoints.Domain.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ModelDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private void SaveFoodType(ModelDbContext context, FoodType foodType)
        {
            if (!context.FoodTypes.Any(x => x.Name == foodType.Name))
            {
                context.FoodTypes.Add(foodType);
            }
        }

        private void SaveRule(ModelDbContext context, DietPointRule rule)
        {
            if (!context.DietPointRules.Any(x => x.Id == rule.Id))
            {
                context.DietPointRules.Add(rule);
            }
        }

        protected override void Seed(ModelDbContext context)
        {
            SaveFoodType(context, new FoodType
            {
                FoodGroup = FoodGroup.Meat,
                Name = "Steak",
                CaloriesMultiplier = 1.4,
                SaturatedFatPercent = 73,
                IsLowFat = false,
                Picture = CreatePicture()
            });
            SaveFoodType(context, new FoodType
            {
                FoodGroup = FoodGroup.Meat,
                Name = "Chicken",
                CaloriesMultiplier = 35,
                SaturatedFatPercent = 30,
                IsLowFat = false,
                Picture = CreatePicture()
            });
            SaveFoodType(context, new FoodType
            {
                FoodGroup = FoodGroup.Vegetables,
                Name = "Salad",
                CaloriesMultiplier = 0.8,
                SaturatedFatPercent = 15,
                IsLowFat = true,
                Picture = CreatePicture()
            });
            SaveFoodType(context, new FoodType
            {
                FoodGroup = FoodGroup.Dairy,
                Name = "Eggs",
                CaloriesMultiplier = 1.1,
                SaturatedFatPercent = 17,
                IsLowFat = true,
                Picture = CreatePicture()
            });
            SaveFoodType(context, new FoodType
            {
                FoodGroup = FoodGroup.Dairy,
                Name = "Cheese",
                CaloriesMultiplier = 1.2,
                SaturatedFatPercent = 30,
                IsLowFat = false,
                Picture = CreatePicture()
            });
            SaveFoodType(context, new FoodType
            {
                FoodGroup = FoodGroup.Grain,
                Name = "Pizza",
                CaloriesMultiplier = 24,
                SaturatedFatPercent = 0,
                IsLowFat = true,
                Picture = CreatePicture()
            });
            SaveFoodType(context, new FoodType
            {
                FoodGroup = FoodGroup.Fruit,
                Name = "Apple",
                CaloriesMultiplier = 0.8,
                SaturatedFatPercent = 0,
                IsLowFat = true,
                Picture = CreatePicture()
            });

            DefineRules(context);

            context.SaveChanges();
        }

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

        private byte[] CreatePicture()
        {
            return new byte[10 * 1000 * 1000];
        }
    }
}