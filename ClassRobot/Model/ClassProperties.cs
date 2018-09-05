/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           03/09/2018
 *      Purpose:        All properties for the deserialized JSON objects
 *      Known Bugs:     nill
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ClassRobot.Model
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   (Serializable) a root object. </summary>
    ///
    /// <remarks>   , 6/09/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    public class RootObject
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the teacher. </summary>
        ///
        /// <value> The teacher. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("teacher")]
        public string Teacher { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier of the class. </summary>
        ///
        /// <value> The identifier of the class. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("class")]
        public string ClassId { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the room. </summary>
        ///
        /// <value> The room. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("room")]
        public string Room { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the date. </summary>
        ///
        /// <value> The date. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("date")]
        public string Date { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the layout. </summary>
        ///
        /// <value> The layout. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("layout")]
        public List<Layout> Layout { get; set; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   (Serializable) a layout. </summary>
    ///
    /// <remarks>   , 6/09/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    public class Layout : IComparable<Layout>
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the horizontal. </summary>
        ///
        /// <value> The horizontal. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("horizontal")]
        public int Horizontal { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the vertical. </summary>
        ///
        /// <value> The vertical. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("vertical")]
        public int Vertical { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets information describing the cell. </summary>
        ///
        /// <value> Information describing the cell. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("cell_data")]
        public string CellData { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets a value indicating whether the colour. </summary>
        ///
        /// <value> True if colour, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("colour")]
        public bool Colour { get; set; }

        //used for RAF

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the size of the record. </summary>
        ///
        /// <value> The size of the record. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonProperty("record_size")]
        public int RecSize { get; set; } = 10;

        //this is used with IComparable to sort an array containing layout objects

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same position
        /// in the sort order as the other object.
        /// </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="other">    An object to compare with this instance. </param>
        ///
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has
        /// these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" />
        /// in the sort order.  Zero This instance occurs in the same position in the sort order as
        /// <paramref name="other" />. Greater than zero This instance follows <paramref name="other" />
        /// in the sort order.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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

