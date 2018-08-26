using ClassRobot.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassRobot.Controller
{
    class RobotController
    {
        public static RootObject AccessDatabase()
        {
            return Json.DeserializeFromFile();
        }

        public static int GetHorizontalCount(List<Layout> layout)
        {
            return layout.Max(x => x.Horizontal) + 1;
        }

        public static int GetVerticalCount(List<Layout> layout)
        {
            return layout.Max(x => x.Vertical) + 2;
        }

        public static void SaveDataToFile(DataGridView dgv_class, RootObject root)
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
                        layout.CellData = "BKGRND_FILL";
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
            root.classes[0].Layout = layouts;

            //save the changes to file
            Json.SerializeToFile(root);
        }

        public static Dictionary<string, object> FindStudent(DataGridView dgv_class, string studentName)
        {
            Dictionary<string, object> tmpDict = new Dictionary<string, object>();
            
            foreach (DataGridViewRow row in dgv_class.Rows)
            {
                //if dictionary contains items, student has been found. Break out of loop
                if (tmpDict.Count == 0)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToUpper() == studentName.ToUpper())
                        {
                            //returns the location and name of the student
                            tmpDict.Add("name", cell.Value);
                            tmpDict.Add("horizontal", cell.ColumnIndex);
                            tmpDict.Add("vertical", cell.RowIndex);

                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            return tmpDict;
        }

        public static DataGridView ClearAllStudents(DataGridView dgv_class)
        {
            foreach (DataGridViewRow row in dgv_class.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value is string && cell.RowIndex > 1)
                    {
                        cell.Value = "";
                    }
                }
            }

            return dgv_class;
        }
    }
}
