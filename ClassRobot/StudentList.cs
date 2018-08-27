using ClassRobot.Controller;
using ClassRobot.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ClassRobot
{
    public partial class StudentList : Form
    {
        MainWindow formCall;
        Layout studentDetails;
        List<Layout> allStudents;
        string searchValue;

        public StudentList()
        {
            InitializeComponent();
        }

        public StudentList(MainWindow callingCall)
        {
            InitializeComponent();
            formCall = callingCall;
        }

        
        public StudentList(MainWindow callingCall, Layout studentDetails, string searchValue)
        {
            InitializeComponent();
            formCall = callingCall;
            this.studentDetails = studentDetails;
            this.searchValue = searchValue;
        }

        private void StudentList_Load(object sender, EventArgs e)
        {
            AddColumns();

            //if student details contains an object, find the index of the student in the list and select them
            if (studentDetails != null)
            {
                int index = StudentListController.SelectStudent(allStudents, searchValue);
                dgv_studentList.CurrentRow.Selected = false;
                dgv_studentList.CurrentCell = dgv_studentList.Rows[index].Cells[0];
                dgv_studentList.Rows[index].Selected = true;
            }
        }

        private void AddColumns()
        {
            allStudents = StudentListController.GetAllStudents((RootObject)Tag);
            dgv_studentList.AutoGenerateColumns = false;
            dgv_studentList.DataSource = allStudents;

            DataGridViewTextBoxColumn studentName = new DataGridViewTextBoxColumn();
            studentName.Name = "Student";
            studentName.DataPropertyName = "CellData";
            studentName.ReadOnly = true;

            DataGridViewTextBoxColumn across = new DataGridViewTextBoxColumn();
            across.Name = "Across";
            across.DataPropertyName = "Horizontal";
            across.ReadOnly = true;

            DataGridViewTextBoxColumn down = new DataGridViewTextBoxColumn();
            down.Name = "Down";
            down.DataPropertyName = "Vertical";
            down.ReadOnly = true;

            dgv_studentList.Columns.Add(studentName);
            dgv_studentList.Columns.Add(across);
            dgv_studentList.Columns.Add(down);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
