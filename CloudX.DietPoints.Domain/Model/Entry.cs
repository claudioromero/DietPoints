using System;

namespace CloudX.DietPoints.Domain.Model
{
    public class Entry
    {
        public virtual int Id { get; set; }
        public virtual string Meal { get; set; }
        public virtual FoodType FoodType { get; set; }
        public virtual int Calories { get; set; }
        public virtual int DietPoints { get; protected set; }
        public virtual DateTime Date { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual void CalculateDietPoints(ModelDbContext context)
        {
            DietPoints = FoodType.CalculateDietPoints(this, context);
        }
    }
}