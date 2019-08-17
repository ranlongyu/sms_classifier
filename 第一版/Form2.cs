using CCWin.SkinClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 第一版
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string pathout = textBox1.Text;
                string pathin="";
                if (radioButton1.Checked) pathin = @"..\Debug\textgather\normal\";
                else if (radioButton2.Checked) pathin = @"..\Debug\textgather\inform\";
                else if (radioButton3.Checked) pathin = @"..\Debug\textgather\spam\";
                else
                {
                    MessageBox.Show("请选择文件或导入选项", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                //从excel导入数据
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + pathout + ";" + "Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                string strExcel = "select * from [Sheet1$]";
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, conn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "table1");
                conn.Close();
                //set.Tables[0].Rows 找到指定表的所有行 0这里可以填表名
                foreach (DataRow col in ds.Tables[0].Rows) 
                {
                    //判断导入目录下文件是否同名
                    int namenum=1;
                    while (File.Exists(string.Format("{0}{1}.txt",pathin, namenum)))
                    {
                        namenum++;
                    }
                    int x=namenum;
                    //成功出来的就是不同名的
                    File.Create(string.Format("{0}{1}.txt", pathin, namenum)).Close();
                    string p = pathin + namenum + ".txt";
                    //将数据写入namenum.txt文件
                    StreamWriter swt = new StreamWriter(p, true, Encoding.Default);
                    swt.WriteLine(col[0]);
                    swt.Close();
                }
                MessageBox.Show("导入成功");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Form2_DragEnter(object sender, DragEventArgs e)
        {
            //获取拖住所得路径
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            textBox1.Text = path; //将获取到的完整路径赋值到textBox1
        }
    }
}
