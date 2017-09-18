using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CloudX.DietPoints.Web.Support
{
    public class PageViewModel<T>
    {
        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}