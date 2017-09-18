using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using CloudX.DietPoints.Domain.Model;

namespace CloudX.DietPoints.Web.Models
{
    public class GetEntriesViewModel
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("dateFrom")]
        [DataType(DataType.DateTime)]
        public DateTime? DateFrom { get; set; }

        [JsonProperty("dateTo")]
        [DataType(DataType.DateTime)]
        public DateTime? DateTo { get; set; }

        [JsonProperty("timeFrom")]
        [DataType(DataType.DateTime)]
        public DateTime? TimeFrom { get; set; }

        [JsonProperty("timeTo")]
        [DataType(DataType.DateTime)]
        public DateTime? TimeTo { get; set; }
    }

    public class EntryViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("meal")]
        [Required]
        public string Meal { get; set; }

        [JsonProperty("calories")]
        [Range(0, 9999)]
        [Required]
        public int? Calories { get; set; }

        [JsonProperty("date")]
        [Required]
        public DateTime Date { get; set; }
        [JsonProperty("foodType")]
        [Required]
        public FoodTypeViewModel FoodType { get; set; }
        [JsonProperty("dietPoints")]
        public double DietPoints { get; set; }
    }

    public class FoodTypeViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}