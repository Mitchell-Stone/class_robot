using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ClassRobot.Model
{
    class RandomAccessFile
    {
        private static int position = 0;
        private static string fileName = @"C:\ClassRobot\student.txt";

        public static void UpdateRafFile(List<Layout> layouts)
        {
            List<Layout> tempList = new List<Layout>();

            foreach (var item in layouts)
            {
                if (item.CellData != null || item.CellData != "")
                {
                    tempList.Add(item);
                }
            }

            try
            {
                File.Create(fileName).Dispose();

                FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);

                //create a binary writer
                BinaryWriter binWriter = new BinaryWriter(stream);

                for (int index = 1; index < tempList.Count; index++)
                {
                    Layout item = tempList[index];

                    //use the size of the string to create the position
                    stream.Position = index * item.RecSize;

                    //write the data
                    binWriter.Write(item.CellData);
                }

                //close the connections
                binWriter.Close();
                stream.Close();
            }
            catch (Exception e)
            {
                //show a message when there is an issue with the file
                MessageBox.Show("There was an issue with file access, please contact your system administrator", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine(e.Message);
            }
        }

        public static string FindRecord(int index)
        {
            Layout layout = new Layout();
            string name;
            //open the file
            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader binReader = new BinaryReader(stream);

                stream.Seek(index * layout.RecSize, SeekOrigin.Begin);

                name = binReader.ReadString().ToString();
               
                stream.Close();
                binReader.Close();

                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
