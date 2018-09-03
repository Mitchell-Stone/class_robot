using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ClassRobot.Model
{
    [Serializable]
    public class RootObject
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
    public class Layout : IComparable<Layout>
    {
        [JsonProperty("horizontal")]
        public int Horizontal { get; set; }
        [JsonProperty("vertical")]
        public int Vertical { get; set; }
        [JsonProperty("cell_data")]
        public string CellData { get; set; }
        [JsonProperty("colour")]
        public bool Colour { get; set; }

        //used for RAF
        [JsonProperty("record_size")]
        public int RecSize { get; set; } = 30;

        //this is used with IComparable to sort an array containing layout objects
        public int CompareTo(Layout other)
        {
            //changes the input to upper case and compares the current friend to the friend entered.
            if (CellData != null)
            {
                return this.CellData.ToUpper().CompareTo(other.CellData.ToUpper());
            }
            else
            {
                return -1;
            }
        }
    } 
}

