namespace CloudX.DietPoints.Domain.Model
{
    public class DietPointRule
    {
        public virtual int Id { get; set; }
        public virtual FoodGroup FoodGroup { get; set; }
        public virtual double MinFatLevel { get; set; }
        public virtual double MaxFatLevel { get; set; }
        public virtual double FixedCalories { get; set; }
        public virtual bool IsLowFat { get; set; }
        public virtual double Multiplier { get; set; }
        public virtual double Bias { get; set; }
    }
}
