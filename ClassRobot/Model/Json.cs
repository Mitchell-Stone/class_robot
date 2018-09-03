using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClassRobot.Model
{
    class Json
    {
        private static string directory = @"C:\ClassRobot\";

        public static RootObject DeserializeFromFile()
        {
            RootObject classes = new RootObject();

            //open last modified file
            List<DateTime> dates = new List<DateTime>();

            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.Contains(".json"))
                {
                    dates.Add(File.GetLastWriteTime(file));
                }              
            }

            var latestDate = dates.Max(r => r.ToString());

            foreach (var file in Directory.GetFiles(directory))
            {
                DateTime dateCheck = File.GetLastWriteTime(file);
                if (dateCheck.ToString() == latestDate)
                {
                    classes = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(file));
                    break;
                }
            }
            return classes;
        }

        public static RootObject DeserializeFromFile(string filePath)
        {
            return JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(filePath));
        }

        public static void SerializeToFile(RootObject classes, DateTime date)
        {
            string fileName = $"class_details_{date.ToString("yyyy-MM-dd")}.json";

            using (StreamWriter writer = new StreamWriter(directory + fileName))
            {
                writer.WriteLine(JsonConvert.SerializeObject(classes, Formatting.Indented));
                //close the connection to the file
                writer.Close();
            }
        }
    }
}
