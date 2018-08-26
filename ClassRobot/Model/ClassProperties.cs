using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ClassRobot.Model
{
    [Serializable]
    public class RootObject
    {
        [JsonProperty("all_classes")]
        public List<Classes> classes { get; set; }
    }

    [Serializable]
    public class Classes
    {
        [JsonProperty("teacher")]
        public string Teacher { get; set; }
        [JsonProperty("class")]
        public string ClassId { get; set; }
        [JsonProperty("room")]
        public string Room { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("layout")]
        public List<Layout> Layout { get; set; }
    }

    [Serializable]
    public class Layout
    {
        [JsonProperty("horizontal")]
        public int Horizontal { get; set; }
        [JsonProperty("vertical")]
        public int Vertical { get; set; }
        [JsonProperty("cell_data")]
        public string CellData { get; set; }
        [JsonProperty("colour")]
        public string Colour { get; set; }
    }
}

