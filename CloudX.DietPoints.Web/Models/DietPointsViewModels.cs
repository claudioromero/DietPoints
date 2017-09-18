using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CloudX.DietPoints.Web.Models
{
    public class DietPointsViewModel
    {
        [JsonProperty("expectedDailyDietPoints")]
        public int ExpectedDailyDietPoints { get; set; }

        [JsonProperty("todaysDietPoints")]
        public int TodaysDietPoints { get; set; }
    }

    public class ConfigureDietPointsViewModel
    {
        [JsonProperty("expectedDailyDietPoints")]
        [Range(0, 9999)]
        [Display(Name = "Expected Daily DietPoints")]
        public int ExpectedDailyDietPoints { get; set; }
    }
}