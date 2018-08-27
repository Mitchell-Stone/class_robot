using ClassRobot.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassRobot.Controller
{
    class StudentListController
    {
        public static List<Layout> GetAllStudents(RootObject root)
        {
            List<Layout> allStudents = new List<Layout>();

            foreach (var items in root.Layout)
            {
                if (items.CellData != null && items.CellData.ToString() != "BKGRND_FILL")
                {
                    allStudents.Add(items);
                }
            }

            return allStudents.OrderBy(x => x.CellData).ToList();
        }

        public static int SelectStudent(List<Layout> allStudents, string searchValue)
        {
            return Array.BinarySearch(allStudents.ToArray(), new Layout() { CellData = searchValue });
        }
    }
}
