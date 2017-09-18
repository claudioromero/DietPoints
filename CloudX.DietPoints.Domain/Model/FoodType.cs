using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudX.DietPoints.Domain.Model
{
    public class FoodType
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double CaloriesMultiplier { get; set; }
        public virtual FoodGroup FoodGroup { get; set; }
        public virtual byte[] Picture { get; set; }
        public virtual double SaturatedFatPercent { get; set; }
        public virtual bool IsLowFat { get; set; }

        public virtual int CalculateDietPoints(Entry entry, ModelDbContext context)
        {
            DietPointRule applicableRule = GetRule(entry, context);

            return (applicableRule == null) ? (int)(entry.Calories * CaloriesMultiplier) : (int)(entry.Calories * applicableRule.Multiplier) + (int)applicableRule.Bias;
        }

        private DietPointRule GetRule(Entry entry, ModelDbContext context)
        {
            DietPointRule applicableRule = null;

            var rules = new List<Func<DietPointRule, bool>>();
            rules.Add(x => x.FoodGroup == entry.FoodType.FoodGroup && entry.FoodType.SaturatedFatPercent >= x.MinFatLevel && entry.FoodType.SaturatedFatPercent <= x.MaxFatLevel);
            rules.Add(x => x.FoodGroup == entry.FoodType.FoodGroup && x.IsLowFat && x.FixedCalories == 0);
            rules.Add(x => x.FoodGroup == entry.FoodType.FoodGroup && x.FixedCalories > 0 && entry.Calories > x.FixedCalories);

            foreach (var rule in rules)
            {
                var p = context.DietPointRules.FirstOrDefault(rule);

                if (p != null)
                {
                    applicableRule = p;
                    break;
                }
            }

            return applicableRule;
        }
    }
}
