/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           03/09/2018
 *      Purpose:        Functions for deserializing and serializing all the data to and from JSON files
 *      Known Bugs:     nill
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClassRobot.Model
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A json. </summary>
    ///
    /// <remarks>   , 6/09/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    class Json
    {
        /// <summary>   . </summary>
        private static string directory = @"C:\ClassRobot\";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deserialize from file. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <returns>   A RootObject. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static RootObject DeserializeFromFile()
        {
            RootObject classes = new RootObject();

            string fileName = "default_class_details.json";
            string defaultDirectory = Path.Combine(directory, fileName);

            if (!File.Exists(defaultDirectory))
            {
                Directory.CreateDirectory(directory);
                //creates a file if one does not exist and fills it with example information
                string[] lines = File.ReadAllLines(@"..\DefaultJson.json");

                using (StreamWriter writer = new StreamWriter(directory + fileName))
                {
                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                    
                    //close the connection to the file
                    writer.Close();
                }
            }

            //open last modified file
            List<DateTime> dates = new List<DateTime>();

            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.Contains(".json"))
                {
                    dates.Add(File.GetLastWriteTime(file));
                }              
            }
            //searches for the latest date
            var latestDate = dates.Max(r => r.ToString());

            foreach (var file in Directory.GetFiles(directory))
            {
                DateTime dateCheck = File.GetLastWriteTime(file);
                if (dateCheck.ToString() == latestDate)
                {
                    //deserializes the information in the latest file
                    classes = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(file));
                    break;
                }
            }
            return classes;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deserialize from file. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="filePath"> Full pathname of the file. </param>
        ///
        /// <returns>   A RootObject. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static RootObject DeserializeFromFile(string filePath)
        {
            //deserializes information from a specific file location
            return JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(filePath));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Serialize to file. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="classes">  The classes. </param>
        /// <param name="date">     The date Date/Time. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void SerializeToFile(RootObject classes, DateTime date)
        {
            string fileName = $"class_details_{date.ToString("yyyy-MM-dd")}.json";

            //write the serialized information to a file
            using (StreamWriter writer = new StreamWriter(directory + fileName))
            {
                writer.WriteLine(JsonConvert.SerializeObject(classes, Formatting.Indented));
                //close the connection to the file
                writer.Close();
            }
        }
    }
}
