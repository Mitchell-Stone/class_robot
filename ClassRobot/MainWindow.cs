/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           03/09/2018
 *      Purpose:        Logic for displaying the data in the main window
 *      Known Bugs:     nill
 */

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

            BuildClassTable();   
        }

        private void BuildClassTable()
        {
            //sets the column count to zero so when reloaded it does not add to existing columns
            dgv_class.ColumnCount = 0;
            //get the row and column count from the json
            int columnCount = MainWindowController.GetHorizontalCount(root.Layout);
            int rowCount = MainWindowController.GetVerticalCount(root.Layout);

            //set the datagrid view settings
            dgv_class.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv_class.ScrollBars = ScrollBars.None;
            dgv_class.RowTemplate.Height = (dgv_class.Height - dgv_class.ColumnHeadersHeight) / rowCount;
            dgv_class.RowHeadersWidth = 50;
            dgv_class.AllowUserToOrderColumns = false;
            dgv_class.AllowUserToResizeColumns = false;
            dgv_class.AllowUserToResizeRows = false;

            //add the columns to the view
            for (int i = 0; i < columnCount + 1; i++)
            {
                dgv_class.Columns.Add(i.ToString(), i.ToString());
            }

            //add the rows to the view
            dgv_class.Rows.Add(rowCount);

            for (int i = 0; i < dgv_class.Rows.Count; i++)
            {
                //numbers the row headers
                dgv_class.Rows[i].HeaderCell.Value = i.ToString();
            }

            //put the information into the data grid view
            PopulateView(root.Layout);

            //make it so the columns can not be sorted
            foreach (DataGridViewColumn dgvc in dgv_class.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //fill the text boxes with relevant information
            tb_teacherName.Text = root.Teacher;
            tb_classId.Text = root.ClassId;
            tb_roomId.Text = root.Room;
            tb_date.Value = DateTime.Parse(root.Date);
        }

        private void PopulateView(List<Layout> layout)
        {
            foreach (var grid in layout)
            {
                var row = dgv_class.Rows[grid.Vertical];

                if (grid.Colour != true)
                {
                    //sets the value of the cell to the data the was entered
                    row.Cells[grid.Horizontal.ToString()].Value = grid.CellData;
                }
                else
                {
                    //sets the colour of the cell to indicate a desk
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
            //collects the information on the screen and sends it to the main controller
            root.Teacher = tb_teacherName.Text;
            root.ClassId = tb_classId.Text;
            root.Room = tb_roomId.Text;
            root.Date = tb_date.Value.ToString("yyyy-MM-dd");
            DateTime date = DateTime.Parse(tb_date.Text);
            MainWindowController.SaveDataToFile(dgv_class, root, date);
        }

        private void dgv_class_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //hihglight the current cell
                rowIndex = dgv_class.HitTest(e.X, e.Y).RowIndex;
                columnIndex = dgv_class.HitTest(e.X, e.Y).ColumnIndex;
                dgv_class.CurrentCell = dgv_class.Rows[rowIndex].Cells[columnIndex];

                //create the context menu
                ContextMenu m = new ContextMenu();

                MenuItem editCell = new MenuItem("Edit Cell");
                MenuItem clearCell = new MenuItem("Clear Cell");
                MenuItem addDesk = new MenuItem("Set as Desk");
                MenuItem removeDesk = new MenuItem("Unset Desk");

                m.MenuItems.Add(editCell);
                m.MenuItems.Add(clearCell);
                m.MenuItems.Add(addDesk);
                m.MenuItems.Add(removeDesk);

                //reference the selection events
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
            //clears the cell of any text
            dgv_class.Rows[rowIndex].Cells[columnIndex].Value = null;
        }

        private void EditCell_Click(object sender, EventArgs e)
        {
            //changes the state of the cell to edit
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

                UpdateStudentList(tb_search.Text);
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

        private void btn_sort_Click(object sender, EventArgs e)
        {
            //opens the student list window
            UpdateStudentList(tb_teacherName.Text);
        }

        private void UpdateStudentList(string studentName)
        {
            //checks to see if the student list form is open
            if (Application.OpenForms.OfType<StudentList>().Count() == 1)
            {
                //if open close the current and open a new form
                Application.OpenForms.OfType<StudentList>().First().Close();
                Form studentList = new StudentList(this, student, studentName);
                studentList.Tag = root;
                studentList.Show();
            }
            else
            {
                //open a new form if one does not exist
                Form studentList = new StudentList(this, student, studentName);
                studentList.Tag = root;
                studentList.Show();
            }
        }

        private void tb_teacherName_TextChanged(object sender, EventArgs e)
        {
            //alters the name in the grid view
            dgv_class.Rows[0].Cells[5].Value = tb_teacherName.Text;
        }

        private void dgv_class_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //changes the cell to edit mode when double clicked
            dgv_class.BeginEdit(true);
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            //opens a window to select a new file to open
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = @"C:\ClassRobot\";
            openFile.RestoreDirectory = true;
            openFile.Filter = "json files (*.json)|*.json";
            openFile.Title = "Select Class File";
            openFile.Multiselect = false;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string location = openFile.FileName;
                root = null;
                root = MainWindowController.AccessDatabase(location);
                BuildClassTable();
            }
        }

        private void btn_RAF_Click(object sender, EventArgs e)
        {
            //updates the random access file to the currently selected layout
            RandomAccessFile.UpdateRafFile(root.Layout);

            try
            {
                //search for the student
                string cellData = RandomAccessFile.FindRecord(Convert.ToInt32(tb_search.Text));
                student = MainWindowController.FindStudent(root, cellData);
                UpdateStudentList(cellData);
                //gets all the information about the student
                Layout layout = MainWindowController.FindStudent(root, cellData);
                //highlight the cell of the student
                dgv_class.CurrentCell = dgv_class.Rows[layout.Vertical].Cells[layout.Horizontal];
           
            }
            catch (Exception)
            {
                if (string.IsNullOrWhiteSpace(tb_search.Text))
                {
                    MessageBox.Show("Please enter a record number and try again.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //name has to match and if not show message
                    MessageBox.Show("There is no student at that record number, please try again.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
