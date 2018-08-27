using ClassRobot.Controller;
using ClassRobot.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClassRobot
{
    public partial class MainWindow : Form
    {
        RootObject root;
        Layout student;
        private int rowIndex;
        private int columnIndex;

        public MainWindow()
        {
            InitializeComponent();

            root = MainWindowController.AccessDatabase();

            BuildClassTabs();   
        }

        private void BuildClassTabs()
        {
            int columnCount = MainWindowController.GetHorizontalCount(root.Layout);
            int rowCount = MainWindowController.GetVerticalCount(root.Layout);

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

            PopulateView(root.Layout);

            foreach (DataGridViewColumn dgvc in dgv_class.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            tb_teacherName.Text = root.Teacher;
            tb_classId.Text = root.ClassId;
            tb_roomId.Text = root.Room;
            dtp_date.Value = DateTime.Parse(root.Date);
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
            root.Teacher = tb_teacherName.Text;
            root.ClassId = tb_classId.Text;
            root.Room = tb_roomId.Text;
            root.Date = dtp_date.Value.ToString("yyyy-MM-dd");
            MainWindowController.SaveDataToFile(dgv_class, root);
        }

        private void dgv_class_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                rowIndex = dgv_class.HitTest(e.X, e.Y).RowIndex;
                columnIndex = dgv_class.HitTest(e.X, e.Y).ColumnIndex;
                dgv_class.CurrentCell = dgv_class.Rows[rowIndex].Cells[columnIndex];

                ContextMenu m = new ContextMenu();

                MenuItem editCell = new MenuItem("Edit Cell");
                MenuItem clearCell = new MenuItem("Clear Cell");
                MenuItem addDesk = new MenuItem("Set as Desk");
                MenuItem removeDesk = new MenuItem("Unset Desk");

                m.MenuItems.Add(editCell);
                m.MenuItems.Add(clearCell);
                m.MenuItems.Add(addDesk);
                m.MenuItems.Add(removeDesk);

                editCell.Click += EditCell_Click;
                clearCell.Click += ClearCell_Click;
                addDesk.Click += AddDesk_Click;
                removeDesk.Click += RemoveDesk_Click;
                
                m.Show(dgv_class, new Point(e.X, e.Y));               
            }
        }

        private void AddDesk_Click(object sender, EventArgs e)
        {
            //adds the color to the cell
            dgv_class.Rows[rowIndex].Cells[columnIndex].Style.BackColor = Color.LightBlue;
            dgv_class.Rows[rowIndex].Cells[columnIndex].Value = null;
        }

        private void RemoveDesk_Click(object sender, EventArgs e)
        {
            //removes the colour from the cell
            dgv_class.Rows[rowIndex].Cells[columnIndex].Style.BackColor = Color.White;
            dgv_class.Rows[rowIndex].Cells[columnIndex].Value = null;
        }

        private void ClearCell_Click(object sender, EventArgs e)
        {
            dgv_class.Rows[rowIndex].Cells[columnIndex].Value = null;
        }

        private void EditCell_Click(object sender, EventArgs e)
        {
            dgv_class.BeginEdit(true);
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            //confirm that they would like to clear all students
            if (MessageBox.Show("Are you sure you want to clear all student names?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //update the datagridview so all student names are removed
                dgv_class = MainWindowController.ClearAllStudents(dgv_class);
            }              
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            try
            {
                //search for the student
                student = MainWindowController.FindStudent(root, tb_search.Text);
                dgv_class.CurrentCell = dgv_class.Rows[student.Vertical].Cells[student.Horizontal];

                UpdateStudentList();
            }
            catch (NullReferenceException)
            {
                if (string.IsNullOrWhiteSpace(tb_search.Text))
                {
                    MessageBox.Show("Please enter a student name.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //name has to match and if not show message
                    MessageBox.Show("There is no student with that name, please try again.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dgv_class_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //thinking about making it enter edit mode when a cell is selected
        }

        private void btn_sort_Click(object sender, EventArgs e)
        {
            UpdateStudentList();
        }

        private void UpdateStudentList()
        {
            //checks to see if the student list form is open
            if (Application.OpenForms.OfType<StudentList>().Count() == 1)
            {
                //if open close the current and open a new form
                Application.OpenForms.OfType<StudentList>().First().Close();
                Form studentList = new StudentList(this, student, tb_search.Text);
                studentList.Tag = root;
                studentList.Show();
            }
            else
            {
                //open a new form if one does not exist
                Form studentList = new StudentList(this, student, tb_search.Text);
                studentList.Tag = root;
                studentList.Show();
            }
        }

        private void tb_teacherName_TextChanged(object sender, EventArgs e)
        {
            dgv_class.Rows[0].Cells[5].Value = tb_teacherName.Text;
        }

        private void dgv_class_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            dgv_class.BeginEdit(true);
        }
    }
}
