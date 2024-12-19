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


namespace Akib_1281742
{
    public partial class MainForm : Form
    {
        BindingSource bsP = new BindingSource();
        BindingSource bsS = new BindingSource();
        DataSet ds;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView2.AutoGenerateColumns =false;
            LoadDataBindingSources();
        }

        public void LoadDataBindingSources()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM courses ", con))
                {
                    ds = new DataSet();
                    da.Fill(ds, "courses");
                    da.SelectCommand.CommandText = @"SELECT   *
                                                    FROM            tutors";

                    da.Fill(ds, "tutors");

                    DataRelation rel = new DataRelation("FK_P_S", ds.Tables["courses"].Columns["courseid"], ds.Tables["tutors"].Columns["courseid"]);
                    ds.Tables["tutors"].Columns.Add(new DataColumn("image", typeof(byte[])));
                    for (int i = 0; i < ds.Tables["tutors"].Rows.Count; i++)
                    {
                        ds.Tables["tutors"].Rows[i]["image"] = File.ReadAllBytes(@"..\..\pictures\" + ds.Tables["tutors"].Rows[i]["picture"]);
                    }
                    ds.Relations.Add(rel);
                    bsP.DataSource = ds;
                    bsP.DataMember = "courses";

                    bsS.DataSource = bsP;
                    bsS.DataMember = "FK_P_S";
                   
                    dataGridView2.DataSource = bsS;
                    AddDataBindings();
                }
            }
        }

        private void AddDataBindings()
        {
            {
                lblid.DataBindings.Clear();
                lblid.DataBindings.Add(new Binding("Text", bsP, "courseid"));
                lblname.DataBindings.Clear();
                lblname.DataBindings.Add(new Binding("Text", bsP, "coursename"));
                lblfee.DataBindings.Clear();
                Binding bp = new Binding("Text", bsP, "fee", true);
                bp.Format += Bp_Format;
                Binding bm = new Binding("Text", bsP, "startdate", true);
                bm.Format += Bm_Format;
                lblstartdate.DataBindings.Clear();
                lblstartdate.DataBindings.Add(bm);
            }
        }

        private void Bm_Format(object sender, ConvertEventArgs e)
        {
            DateTime d = (DateTime)e.Value;
            e.Value = d.ToString("yyyy-MM-dd");
        }

        private void Bp_Format(object sender, ConvertEventArgs e)
        {
            DateTime d = (DateTime)e.Value;
            e.Value = d.ToString("yyyy-MM-dd");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bsP.Position < bsP.Count - 1)
            {
                bsP.MoveNext();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bsP.Position > 0)
            {
                bsP.MovePrevious();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            bsP.MoveFirst();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bsP.MoveLast();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddForm { TheForm = this }.ShowDialog();
        }

        private void report1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmRpt1().Show();
        }

        private void subReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmRpt2().Show();
        }
    }
}
