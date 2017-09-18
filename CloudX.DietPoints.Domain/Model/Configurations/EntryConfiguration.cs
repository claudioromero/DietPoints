using System.Data.Entity.ModelConfiguration;

namespace CloudX.DietPoints.Domain.Model.Configurations
{
    public class EntryConfiguration : EntityTypeConfiguration<Entry>
    {
        public EntryConfiguration()
        {
            HasKey(x => x.Id);
            HasRequired(x => x.FoodType).WithMany().Map(x => x.MapKey("FoodTypeId"));
            HasRequired(x => x.User).WithMany().Map(x => x.MapKey("UserId"));
        }
    }
}
