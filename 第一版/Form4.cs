using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 第一版
{
    public partial class Form4 : Form
    {

        public Form4()
        {
            InitializeComponent();           
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“短信DataSet.notyet”中。您可以根据需要移动或删除它。
            this.notyetTableAdapter.Fill(this.短信DataSet.notyet);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //进行计时
            Stopwatch time = new Stopwatch();
            time.Start();
            //数据库连接
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\Debug\短信.mdb";
            OleDbConnection connection = new OleDbConnection(conStr);
            OleDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from notyet";
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);     
            //创建commandbuider对象
            OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(adapter);
            adapter.InsertCommand = cmdBuilder.GetInsertCommand();
            adapter.DeleteCommand = cmdBuilder.GetDeleteCommand();
            adapter.UpdateCommand = cmdBuilder.GetUpdateCommand();
            //创建datatable对象
            DataTable dt = new DataTable();
            adapter.Fill(dt);          
            foreach (DataRow row in dt.Rows)
            {
                bayes a = new bayes();
                a.Compute(row["sms"].ToString());
                if (a.getnorchance > a.getinfchance && a.getnorchance > a.getspachance)
                    row["bayes分类"] = "正常短信";
                else if (a.getinfchance > a.getnorchance && a.getinfchance > a.getspachance)
                    row["bayes分类"] = "通知短信";
                else row["bayes分类"] = "垃圾短信";
            }
            //数据提交到数据库
            adapter.Update(dt);
            //更新datagridview数据
            this.notyetTableAdapter.Fill(this.短信DataSet.notyet);
            //计时
            time.Stop();
            TimeSpan ts2 = time.Elapsed;
            MessageBox.Show("分类完成！\n耗时："+ts2.TotalSeconds.ToString()+"秒","提示");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //进行计时
            Stopwatch time = new Stopwatch();
            time.Start();
            //数据库连接
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\Debug\短信.mdb";
            OleDbConnection connection = new OleDbConnection(conStr);
            OleDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from notyet";
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            //创建commandbuider对象
            OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(adapter);
            adapter.InsertCommand = cmdBuilder.GetInsertCommand();
            adapter.DeleteCommand = cmdBuilder.GetDeleteCommand();
            adapter.UpdateCommand = cmdBuilder.GetUpdateCommand();
            //创建datatable对象
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                knn a = new knn();
                a.Compute(row["sms"].ToString());
                if (a.getnorchance > a.getinfchance && a.getnorchance > a.getspachance)
                    row["knn分类"] = "正常短信";
                else if (a.getinfchance > a.getnorchance && a.getinfchance > a.getspachance)
                    row["knn分类"] = "通知短信";
                else row["knn分类"] = "垃圾短信";
            }
            //数据提交到数据库
            adapter.Update(dt);
            //更新datagridview数据
            this.notyetTableAdapter.Fill(this.短信DataSet.notyet);
            //计时
            time.Stop();
            TimeSpan ts2 = time.Elapsed;
            MessageBox.Show("分类完成！\n耗时：" + ts2.TotalSeconds.ToString() + "秒", "提示");           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //提交数据更改到数据库
            this.notyetTableAdapter.Update(this.短信DataSet);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //滚回所有更改
            this.短信DataSet.RejectChanges();
        }
    }
}
