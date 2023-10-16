using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab05.Model;

//Nguyen Thanh Truong da o day
namespace Lab05
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                StudentDBContext context = new StudentDBContext();
                List<Faculty> listFalcultys = context.Faculties.ToList(); //lấy các khoa
                List<Student> listStudent = context.Students.ToList(); //lấy sinh viên
                List<Major> listMajor = context.Majors.ToList();
                FillFalcultyCombobox(listFalcultys); BindGrid(listStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillFalcultyCombobox(List<Faculty> listFalcultys)
        {
            this.comboBox1.DataSource = listFalcultys;
            this.comboBox1.DisplayMember = "FacultyName";
            this.comboBox1.ValueMember = "FacultyID";
        }
        //Hàm binding gridView từ list sinh viên
        private void BindGrid(List<Student> listStudent)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.StudentID;
                dataGridView1.Rows[index].Cells[1].Value = item.FullName;
                dataGridView1.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dataGridView1.Rows[index].Cells[3].Value = item.AverageScore;
                dataGridView1.Rows[index].Cells[4].Value = item.Major.Name;
            }
        }
            private void button1_Click(object sender, EventArgs e)
        {
            StudentDBContext db = new StudentDBContext();
            var mssv = textBox1.Text;
            var ten = textBox2.Text;
            var diem = textBox3.Text;
            var khoa = (int)comboBox1.SelectedValue;

            Student s = new Student()
            {
                StudentID = mssv,
                FullName = ten,
                AverageScore = double.Parse(diem),
                FacultyID = khoa

            };
            db.Students.Add(s);
            db.SaveChanges();
            BindGrid(db.Students.ToList());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StudentDBContext db = new StudentDBContext();
            var updateStudent = db.Students.SingleOrDefault(c => c.StudentID.Equals(textBox1.Text)); if (updateStudent == null)
            {
                MessageBox.Show("Không tồn tại sinh viên có MSSV {0}", textBox1.Text);
                return;
            }
            updateStudent.StudentID = textBox1.Text;
            updateStudent.FullName = textBox2.Text;
            updateStudent.AverageScore = double.Parse(textBox3.Text);
            updateStudent.FacultyID = (int)comboBox1.SelectedValue;

            db.SaveChanges();
            BindGrid(db.Students.ToList());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                StudentDBContext student = new StudentDBContext();
                string studentID = textBox1.Text;
                Student selectedStudent = student.Students.FirstOrDefault(s => s.StudentID == studentID);

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    student.Students.Remove(selectedStudent);
                    student.SaveChanges();
                    MessageBox.Show("Xoa sinh vien thanh cong");
                    List<Student> listStudents = student.Students.ToList();
                    BindGrid(listStudents);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string imageLocation = "M:/Images";
            try
            {
                OpenFileDialog fileOpen = new OpenFileDialog();
                fileOpen.Title = "Chọn ảnh";
                fileOpen.Filter = "Hình ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files(*.*)|*.*";

                if (fileOpen.ShowDialog() == DialogResult.OK)
                {
                    imageLocation = fileOpen.FileName;
                    pictureBox1.Image = Image.FromFile(imageLocation);
                    //pathImage = imageLocation;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không thể upload ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
