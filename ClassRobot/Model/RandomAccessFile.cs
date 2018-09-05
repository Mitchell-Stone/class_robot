/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           03/09/2018
 *      Purpose:        Functions for writing to and searching in a random access file
 *      Known Bugs:     nill
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ClassRobot.Model
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A random access file. </summary>
    ///
    /// <remarks>   , 6/09/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    class RandomAccessFile
    {
        /// <summary>   Filename of the file. </summary>
        private static string fileName = @"C:\ClassRobot\student.dat";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the raf file described by layouts. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="layouts">  The layouts. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void UpdateRafFile(List<Layout> layouts)
        {
            try
            {
                //create a new file each time so it is up to date with the latest data in the window
                File.Create(fileName).Dispose();

                using (FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                using (BinaryWriter binWriter = new BinaryWriter(stream))
                {
                    int index = 1;

                    foreach (var item in layouts)
                    {                   
                        //write the data
                        if (item.CellData != null)
                        {
                            //use the size of the string to create the position
                            stream.Position = index * item.RecSize;

                            binWriter.Write(item.CellData);

                            index++;
                        }
                    }

                    //close the connections
                    binWriter.Close();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                //show a message when there is an issue with the file
                MessageBox.Show("There was an issue with file access, please contact your system administrator", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine(e.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Searches for the first record. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="index">    Zero-based index of the. </param>
        ///
        /// <returns>   The found record. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string FindRecord(int index)
        {
            Layout layout = new Layout();
            string name;
            
            try
            {
                //open the file
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                using (BinaryReader binReader = new BinaryReader(stream))
                {
                    //seek for the infromation in the indexed location
                    stream.Seek(index * layout.RecSize, SeekOrigin.Begin);
                    //return the data as a string
                    name = binReader.ReadString().ToString();
                    //close the connection
                    stream.Close();
                    binReader.Close();

                    return name;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
