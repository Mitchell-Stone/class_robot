﻿using ClassRobot.Controller;
using ClassRobot.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassRobot
{
    public partial class Form1 : Form
    {
        RootObject root;

        public Form1()
        {
            InitializeComponent();
            
            //WindowState = FormWindowState.Maximized;

            root = RobotController.AccessDatabase();

            BuildClassTabs();   
        }

        private void BuildClassTabs()
        {
            foreach (var cl in root.classes)
            {

                int columnCount = RobotController.GetHorizontalCount(cl.Layout);
                int rowCount = RobotController.GetVerticalCount(cl.Layout);

                dgv_class.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv_class.ScrollBars = ScrollBars.None;
                dgv_class.RowTemplate.Height = (dgv_class.Height - dgv_class.ColumnHeadersHeight) / rowCount;
                dgv_class.RowHeadersWidth = 50;
                dgv_class.AllowUserToOrderColumns = false;
                dgv_class.AllowUserToResizeColumns = false;
                dgv_class.AllowUserToResizeRows = false;

                for (int i = 0; i < columnCount + 1; i++)
                {
                    dgv_class.Columns.Add(i.ToString(), i.ToString());
                }

                dgv_class.Rows.Add(rowCount);

                for (int i = 0; i < dgv_class.Rows.Count; i++)
                {
                    dgv_class.Rows[i].HeaderCell.Value = i.ToString();
                }

                PopulateView(cl.Layout);

                lbl_teacherName.Text = cl.Teacher;
                lbl_classId.Text = cl.ClassId;
                lbl_roomId.Text = cl.Room;
                lbl_date.Text = DateTime.Today.ToString("dd-MM-yyyy");
            }
        }

        private void PopulateView(List<Layout> layout)
        {
            foreach (var grid in layout)
            {
                var row = dgv_class.Rows[grid.Vertical];

                if (grid.CellData != "BKGRND_FILL")
                {
                    row.Cells[grid.Horizontal.ToString()].Value = grid.CellData;
                }
                else
                {
                    row.Cells[grid.Horizontal.ToString()].Style.BackColor = Color.LightBlue;
                }               
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            RobotController.SaveDataToFile(dgv_class, root);
        }

        private int rowIndex;
        private int columnIndex;

        private void dgv_class_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                rowIndex = dgv_class.HitTest(e.X, e.Y).RowIndex;
                columnIndex = dgv_class.HitTest(e.X, e.Y).ColumnIndex;
                dgv_class.CurrentCell = dgv_class.Rows[rowIndex].Cells[columnIndex];
                //var color = dgv_class.Rows[rowIndex].Cells[columnIndex].Style.BackColor;

                ContextMenu m = new ContextMenu();

                MenuItem addDesk = new MenuItem("Set as Desk");
                MenuItem removeDesk = new MenuItem("Unset Desk");

                m.MenuItems.Add(addDesk);
                m.MenuItems.Add(removeDesk);

                addDesk.Click += AddDesk_Click;
                removeDesk.Click += RemoveDesk_Click;

                m.Show(dgv_class, new Point(e.X, e.Y));               
            }
        }

        private void AddDesk_Click(object sender, EventArgs e)
        {
            dgv_class.Rows[rowIndex].Cells[columnIndex].Style.BackColor = Color.LightBlue;
            dgv_class.Rows[rowIndex].Cells[columnIndex].Value = null;
        }

        private void RemoveDesk_Click(object sender, EventArgs e)
        {
            dgv_class.Rows[rowIndex].Cells[columnIndex].Style.BackColor = Color.White;
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            //confirm that they would like to clear all students
            if (MessageBox.Show("Are you sure you want to clear all student names?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //update the datagridview so all student names are removed
                dgv_class = RobotController.ClearAllStudents(dgv_class);
            }              
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object> studentDetails = RobotController.FindStudent(dgv_class, tb_search.Text);
                dgv_class.CurrentCell = dgv_class.Rows[Convert.ToInt32(studentDetails["vertical"])]
                    .Cells[Convert.ToInt32(studentDetails["horizontal"])];
            }
            catch (Exception)
            {
                MessageBox.Show("There is no student with that name, please try again.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
