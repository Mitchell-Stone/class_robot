using Newtonsoft.Json;
using System.IO;

namespace ClassRobot.Model
{
    class Json
    {
        private static string directory = @"C:\ClassRobot\class_details.txt";

        public static RootObject DeserializeFromFile()
        {
            RootObject classes = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(directory));
            return classes;
        }

        public static void SerializeToFile(RootObject classes)
        {
            using (StreamWriter writer = new StreamWriter(directory))
            {
                writer.WriteLine(JsonConvert.SerializeObject(classes, Formatting.Indented));
                //close the connection to the file
                writer.Close();
            }
        }
    }
}
