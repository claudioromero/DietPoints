using System.Data.Entity.ModelConfiguration;

namespace CloudX.DietPoints.Domain.Model.Configurations
{
    public class DietPointRulesConfiguration : EntityTypeConfiguration<DietPointRule>
    {
        public DietPointRulesConfiguration()
        {
            HasKey(x => x.Id);
        }
    }
}
