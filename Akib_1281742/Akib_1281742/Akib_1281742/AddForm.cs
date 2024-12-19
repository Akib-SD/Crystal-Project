using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Akib_1281742
{
    public partial class AddForm : Form
    {
        List<tutor> tutors = new List<tutor>();
        string currentFile = "";
        public AddForm()
        {
            InitializeComponent();
        }
        public MainForm TheForm { get; set; }

        private void AddForm_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            LoadTutorCombo();
        }

        private void LoadTutorCombo()
        {
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            //{
            //    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tutors ", con))
            //    {
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);

            //        comboBox1.DataSource = dt;
            //    }
            //}
        }
        public class tutor
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Picture { get; set; }
           public bool available {  get; set; }
            public string PictureFullPath { get; set; }
            public byte[] Image { get; set; }

        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFile = openFileDialog1.FileName;
               label7.Text = Path.GetFileName(currentFile);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(currentFile == "") return;
            tutor s = new tutor { Name = textBox3.Text, Phone = textBox2.Text, PictureFullPath = currentFile, Picture = Path.GetFileName(currentFile), available=checkBox1.Checked };
            s.Image = File.ReadAllBytes(currentFile);
            tutors.Add(s);
            currentFile = "";
            label7.Text = "";
            textBox2.Clear();
            textBox3.Clear();
            checkBox1 .Checked = false;
            numericUpDown1.Value = 0;
            BindDataToGrid();
        }

        private void BindDataToGrid()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = tutors;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                tutors.RemoveAt(e.RowIndex);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = tutors;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                con.Open();
                using (SqlTransaction trx = con.BeginTransaction())
                {
                    string sql = "INSERT INTO courses ( coursename, fee ,startdate) VALUES(@cn, @fee, @sd); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(sql, con, trx))
                    {
                        cmd.Parameters.AddWithValue("@cn", textBox1.Text);
                        cmd.Parameters.AddWithValue("@fee", numericUpDown1.Value);
                        cmd.Parameters.AddWithValue("@sd", dateTimePicker1.Value);
                    
                        try
                        {
                            object tid = cmd.ExecuteScalar();
                            foreach (var s in tutors)
                            {

                                string ext = Path.GetExtension(s.Picture);
                                string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                                string savePath = @"..\..\Pictures\" + f;
                                FileStream fs = new FileStream(savePath, FileMode.Create);
                                fs.Write(s.Image, 0, s.Image.Length);
                                fs.Close();
                                cmd.CommandText = "INSERT INTO tutors (tutorname, phone, picture,available, courseid ) VALUES (@tn,  @p, @pic,@av, @ci)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@tn", s.Name);
                                
                                cmd.Parameters.AddWithValue("@p", s.Phone);
                               
                                cmd.Parameters.AddWithValue("@pic", f);
                                cmd.Parameters.AddWithValue("@av", s.available );

                                cmd.Parameters.AddWithValue("@ci", tid);
                                cmd.ExecuteNonQuery();

                            }
                            trx.Commit();
                            MessageBox.Show("Data saved", "Success");
                            textBox1.Clear();
                            textBox2.Clear();
                            tutors.Clear();
                            BindDataToGrid();
                            TheForm.LoadDataBindingSources(); 

                        }
                        catch (Exception ex)
                        {
                            trx.Rollback();
                            MessageBox.Show("ERR: " + ex.Message, "Error");
                        }
                    }
                }
            }
        }
    }
}
