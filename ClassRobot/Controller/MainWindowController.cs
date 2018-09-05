/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           03/09/2018
 *      Purpose:        The controller for the main window that executes all the functions needed to collate the data
 *      Known Bugs:     nill
 */

using ClassRobot.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClassRobot.Controller
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A controller for handling main windows. </summary>
    ///
    /// <remarks>   , 6/09/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    class MainWindowController
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Access database. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <returns>   A RootObject. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static RootObject AccessDatabase()
        {
            //deserializes the json infomation
            return Json.DeserializeFromFile();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Access database. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="fileName"> Filename of the file. </param>
        ///
        /// <returns>   A RootObject. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static RootObject AccessDatabase(string fileName)
        {
            //deserializes the json infomation in a location
            return Json.DeserializeFromFile(fileName);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets horizontal count. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="layout">   The layout. </param>
        ///
        /// <returns>   The horizontal count. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int GetHorizontalCount(List<Layout> layout)
        {
            //looks for the highest horizontal value and adds 1 to make room
            return layout.Max(x => x.Horizontal) + 1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets vertical count. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="layout">   The layout. </param>
        ///
        /// <returns>   The vertical count. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int GetVerticalCount(List<Layout> layout)
        {
            //looks for the highest vertical value and adds 2 to make room
            return layout.Max(x => x.Vertical) + 2;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a data to file. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="dgv_class">    The dgv class. </param>
        /// <param name="root">         The root. </param>
        /// <param name="date">         The date Date/Time. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void SaveDataToFile(DataGridView dgv_class, RootObject root, DateTime date)
        {
            Layout layout;

            List<Layout> layouts = new List<Layout>();

            //iterates over the rows in the 
            foreach (DataGridViewRow row in dgv_class.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Style.BackColor == Color.LightBlue)
                    {
                        //creates a new object for a cell that only contains a colour
                        layout = new Layout();
                        layout.Colour = true;
                        layout.Horizontal = cell.ColumnIndex;
                        layout.Vertical = cell.RowIndex;

                        layouts.Add(layout);
                    }
                    else if (cell.Value != null)
                    {
                        //creates an object for a cell that contains actual data
                        layout = new Layout();
                        layout.CellData = cell.Value.ToString();
                        layout.Horizontal = cell.ColumnIndex;
                        layout.Vertical = cell.RowIndex;

                        layouts.Add(layout);
                    }
                }
            }

            //the index of this could come from a parameter if there were more classes
            root.Layout = layouts;

            //save the changes to file
            Json.SerializeToFile(root, date);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Searches for the first student. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="root">     The root. </param>
        /// <param name="cellData"> Information describing the cell. </param>
        ///
        /// <returns>   The found student. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Layout FindStudent(RootObject root, string cellData)
        {
            List<Layout> layouts = new List<Layout>();
            
            //add all the cell data to the layout list

            foreach (var layout in root.Layout)
            {
                layouts.Add(layout);
            }
            
            //sort the array in the order of the cell data
            Layout[] sortedArray = layouts.OrderBy(x => x.CellData).ToArray();

            //conducst the binary search to find the location of the student
            Layout search = new Layout() { CellData = cellData };

            int index = Array.BinarySearch(sortedArray, search);

            //if the student exists in the layout, index will be a positive int
            if (index >= 0)
            {
                //the int returned from the search is the location of where the student is in the sorted list
                return sortedArray[index];
            }
            else
            {
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clears all students described by dgv_class. </summary>
        ///
        /// <remarks>   , 6/09/2018. </remarks>
        ///
        /// <param name="dgv_class">    The dgv class. </param>
        ///
        /// <returns>   A DataGridView. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DataGridView ClearAllStudents(DataGridView dgv_class)
        {
            //goes over each cell and removes the student information
            foreach (DataGridViewRow row in dgv_class.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value is string && cell.RowIndex > 1)
                    {
                        cell.Value = null;
                    }
                }
            }

            return dgv_class;
        }
    }
}
