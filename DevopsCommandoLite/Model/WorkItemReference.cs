using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsCommandoLite.Terminal.Services
{
    public class WorkItemReference
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }

    public class WorkItemReferenceParent
    {
        public IEnumerable<WorkItemReference> Value { get; set; }
    }
}
