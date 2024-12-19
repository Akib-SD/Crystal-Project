using Akib_1281742.Reports;
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
    public partial class FrmRpt2 : Form
    {
        public FrmRpt2()
        {
            InitializeComponent();
        }

        private void FrmRpt2_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM courses ", con))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "courses");
                    da.SelectCommand.CommandText = @"SELECT   *
                                                    FROM            tutors";

                    da.Fill(ds, "tutors1");


                    ds.Tables["tutors1"].Columns.Add(new DataColumn("image", typeof(byte[])));
                    for (int i = 0; i < ds.Tables["tutors1"].Rows.Count; i++)
                    {
                        ds.Tables["tutors1"].Rows[i]["image"] = File.ReadAllBytes(@"..\..\pictures\" + ds.Tables["tutors1"].Rows[i]["picture"]);
                    }
                    CrystalReport2 rpt = new CrystalReport2();
                    rpt.SetDataSource(ds);
                    crystalReportViewer1.ReportSource = rpt;
                    rpt.Refresh();
                    crystalReportViewer1.Refresh();


                }
            }
        }
    }
}
