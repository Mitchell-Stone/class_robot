﻿/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           03/09/2018
 *      Purpose:        The controller for the student list window that executes all the functions needed to collate the data
 *      Known Bugs:     nill
 */

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
            //create a list of all the cells that contain data
            foreach (var items in root.Layout)
            {
                if (items.CellData != null)
                {
                    allStudents.Add(items);
                }
            }
            //sort them alphabetically
            return allStudents.OrderBy(x => x.CellData).ToList();
        }

        public static int SelectStudent(List<Layout> allStudents, string searchValue)
        {
            //conduct a binary search to find the index of a student in the student list window
            return Array.BinarySearch(allStudents.ToArray(), new Layout() { CellData = searchValue });
        }
    }
}
