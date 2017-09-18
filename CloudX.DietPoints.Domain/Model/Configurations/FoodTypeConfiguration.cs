using System.Data.Entity.ModelConfiguration;

namespace CloudX.DietPoints.Domain.Model.Configurations
{
    public class FoodTypeConfiguration : EntityTypeConfiguration<FoodType>
    {
        public FoodTypeConfiguration()
        {
            HasKey(x => x.Id);
        }
    }
}
