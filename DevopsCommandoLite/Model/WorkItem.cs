using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsCommandoLite.Terminal.Services
{

    public class WorkItems
    {
        public int Count { get; set; }


        public IList<WorkItem> Value { get; set; }
    }

    public class WorkItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }


        public WorkItemFields Fields { get; set; }
    }

    public class WorkItemFields
    {
        [JsonProperty(PropertyName = "System.Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "System.WorkItemType")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "System.CreatedBy")]
        public WorkItemCreatedByField CreatedBy { get; set; }
    }


    public class WorkItemCreatedByField
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UniqueName { get; set; }
        public string ImageUrl { get; set; }
    }
}
