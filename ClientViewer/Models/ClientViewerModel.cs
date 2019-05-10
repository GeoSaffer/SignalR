using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ClientViewer.Models
{
    public class ClientViewerModel
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonIgnore]
        public string LastUpdatedBy { get; set; }
    }
}