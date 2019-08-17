using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JiebaNet;
using JiebaNet.Segmenter;
using System.IO;
using JiebaNet.Analyser;
using System.Diagnostics;

namespace 第一版
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //进行计时
                Stopwatch time = new Stopwatch();
                time.Start();
                //将inform,normal,spam三个文件夹下的文件汇入gather文件夹下的collect.txt文档
                File.Create(@"..\Debug\textgather\gather\collect.txt").Close();
                string[] subdirectory = Directory.GetFiles(@"..\Debug\textgather\spam\");
                foreach (string path in subdirectory) {
                    StreamReader sr = new StreamReader(path,Encoding.Default);
                    string content = sr.ReadLine();
                    sr.Close();
                    StreamWriter sw = new StreamWriter(@"..\Debug\textgather\gather\collect.txt", true, Encoding.Default);
                    sw.WriteLine(content);
                    sw.Close();
                }
                subdirectory = Directory.GetFiles(@"..\Debug\textgather\inform\");
                foreach (string path in subdirectory)
                {
                    StreamReader sr = new StreamReader(path, Encoding.Default);
                    string content = sr.ReadLine();
                    sr.Close();
                    StreamWriter sw = new StreamWriter(@"..\Debug\textgather\gather\collect.txt", true, Encoding.Default);
                    sw.WriteLine(content);
                    sw.Close();
                }
                subdirectory = Directory.GetFiles(@"..\Debug\textgather\normal\");
                foreach (string path in subdirectory)
                {
                    StreamReader sr = new StreamReader(path, Encoding.Default);
                    string content = sr.ReadLine();
                    sr.Close();
                    StreamWriter sw = new StreamWriter(@"..\Debug\textgather\gather\collect.txt", true, Encoding.Default);
                    sw.WriteLine(content);
                    sw.Close();
                }
                //将collect文本中的词语进行分词，以及用TF-IDF算法提取关键词保存在collectok文本
                StreamReader srt = new StreamReader(@"..\Debug\textgather\gather\collect.txt", Encoding.Default);
                string cont = srt.ReadToEnd();
                srt.Close();
                //调用jieba提取仅包含名词和动词的关键词
                var extractor = new TfidfExtractor();
                var keywords = extractor.ExtractTagsWithWeight(cont,6000, Constants.NounAndVerbPos);
                File.Create(@"..\Debug\textgather\gather\collecto.txt").Close();
                StreamWriter swt = new StreamWriter(@"..\Debug\textgather\gather\collecto.txt", true, Encoding.Default);
                string strline=null;
                foreach (var keyword in keywords)
                {
                    strline = keyword.Word+"    "+keyword.Weight;
                    swt.WriteLine(strline);
                }
                swt.Close();
                //计时
                time.Stop();
                TimeSpan ts2 = time.Elapsed;
                listBox1.Items.Clear();
                listBox1.Items.Add("文本预处理完成，总耗时：" + ts2.TotalSeconds + "秒");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //进行计时
                Stopwatch time = new Stopwatch();
                time.Start();
                //创建训练文本
                File.Create(@"..\Debug\textgather\gather\train.txt").Close();
                //获取三类文件的个数,方便计算概率
                string[] files = Directory.GetFiles(@"..\Debug\textgather\normal\");
                int firstnum = files.Length;
                files = Directory.GetFiles(@"..\Debug\textgather\inform\");
                int secondnum = files.Length;
                files = Directory.GetFiles(@"..\Debug\textgather\spam\");
                int thirdnum = files.Length;
                //遍历collectok.txt文档的每一行
                foreach (string strs in File.ReadAllLines(@"..\Debug\textgather\gather\collectok.txt",Encoding.Default))
                {
                    //将strs中第一个词提取出来作为str
                    int i = strs.IndexOf(" ");
                    String str = strs.Substring(0, i);
                    //定义三个变量分别为每个文件夹出现的次数
                    int first = 0;
                    int second = 0;
                    int third = 0;
                    //遍历第一类normal文件夹里每一个文档
                    string[] subdirectory = Directory.GetFiles(@"..\Debug\textgather\normal\");
                    foreach (string path in subdirectory)
                    {                      
                        string text = File.ReadAllText(path, Encoding.Default);
                        if (text.Contains(str))   first++;
                    }
                    //遍历第二类inform文件夹里每一个文档
                    subdirectory = Directory.GetFiles(@"..\Debug\textgather\inform\");
                    foreach (string path in subdirectory)
                    {
                        string text = File.ReadAllText(path, Encoding.Default);
                        if (text.Contains(str)) second++;
                    }
                    //遍历第三类spam文件夹里每一个文档
                    subdirectory = Directory.GetFiles(@"..\Debug\textgather\spam\");
                    foreach (string path in subdirectory)
                    {
                        string text = File.ReadAllText(path, Encoding.Default);
                        if (text.Contains(str)) third++;
                    }
                    //用拉普拉斯估计计算概率，分子加一
                    first++;
                    second++;
                    third++;
                    float firstchance = (float)first / firstnum;
                    float secondchance = (float)second / secondnum;
                    float thirdchance = (float)third / thirdnum;
                    //将已训练的某一行数据写入train.txt文件
                    StreamWriter swt = new StreamWriter(@"..\Debug\textgather\gather\train.txt", true, Encoding.Default);
                    string strline = str+" "+firstchance+" "+secondchance+" "+thirdchance;
                    swt.WriteLine(strline);
                    swt.Close();                
                }
                //计时
                time.Stop();
                TimeSpan ts2 = time.Elapsed;
                listBox1.Items.Clear();
                listBox1.Items.Add("贝叶斯训练完成，总耗时：");
                listBox1.Items.Add(ts2.TotalMinutes + "分");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {            
            try
            {
                //进行计时
                Stopwatch time = new Stopwatch();
                time.Start();
                //读入textBox1的短信，并开始分词
                string content = textBox1.Text;
                //遍历训练好的train文档的每一行，计算文本属于类一，类二，类三的总似然
                bayes a = new bayes();
                a.Compute(content);
                //计时
                time.Stop();
                TimeSpan ts2 = time.Elapsed;
                listBox1.Items.Clear();
                listBox1.Items.Add( "为正常短信概率为：" + (100 *a.getnorchance).ToString() + "%");
                listBox1.Items.Add( "为通知短信概率为：" + (100 * a.getinfchance).ToString() + "%");
                listBox1.Items.Add( "为垃圾短信概率为：" + (100 * a.getspachance).ToString() + "%");
                listBox1.Items.Add("贝叶斯测试总耗时：" + ts2.TotalSeconds + "秒");
                if(a.getnorchance> a.getinfchance && a.getnorchance > a.getspachance)
                    listBox1.Items.Add("综合评估为：正常短信");
                else if (a.getinfchance > a.getnorchance && a.getinfchance > a.getspachance)
                    listBox1.Items.Add("综合评估为：通知短信");
                else listBox1.Items.Add("综合评估为：垃圾短信");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //进行计时
                Stopwatch time = new Stopwatch();
                time.Start();
                //创建训练文本
                File.Create(@"..\Debug\textgather\gather\trainknn.txt").Close();
                //遍历collect.txt文档的每一行
                foreach (string strs in File.ReadAllLines(@"..\Debug\textgather\gather\collect.txt", Encoding.Default))
                {
                    if (strs.Length <= 0) continue;
                    string weight = "";
                    //遍历collectok.txt文档取特征词
                    foreach (string words in File.ReadAllLines(@"..\Debug\textgather\gather\collectok.txt", Encoding.Default))
                    {
                        //将words中第一个词提取出来作为word
                        int i = words.IndexOf(" ");
                        String word = words.Substring(0, i);
                        //查找word在strs中的出现次数
                        string s1 = strs.Replace(word, "");
                        int number = (strs.Length - s1.Length) / word.Length;
                        weight = weight + number + " ";
                    }
                    //将已训练的某一行数据写入trainknn.txt文件
                    StreamWriter swt = new StreamWriter(@"..\Debug\textgather\gather\trainknn.txt", true, Encoding.Default);
                    swt.WriteLine(weight);
                    swt.Close();   
                }
                //计时
                time.Stop();
                TimeSpan ts2 = time.Elapsed;
                listBox1.Items.Clear();
                listBox1.Items.Add("KNN训练完成，总耗时：");
                listBox1.Items.Add(ts2.TotalMinutes + "分");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form4 a = new Form4();
            a.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //进行计时
            Stopwatch time = new Stopwatch();
            time.Start();
            //读取短信
            string content = textBox1.Text;
            knn a = new knn();
            a.Compute(content);
            //计时
            time.Stop();
            TimeSpan ts2 = time.Elapsed;

            listBox1.Items.Clear();
            listBox1.Items.Add("为正常短信概率为：" + (100 * a.getnorchance).ToString() + "%");
            listBox1.Items.Add("为通知短信概率为：" + (100 * a.getinfchance).ToString() + "%");
            listBox1.Items.Add("为垃圾短信概率为：" + (100 * a.getspachance).ToString() + "%");
            listBox1.Items.Add("KNN测试总耗时：" + ts2.TotalSeconds + "秒");
            if (a.getnorchance > a.getinfchance && a.getnorchance > a.getspachance)
                listBox1.Items.Add("综合评估为：正常短信");
            else if (a.getinfchance > a.getnorchance && a.getinfchance > a.getspachance)
                listBox1.Items.Add("综合评估为：通知短信");
            else listBox1.Items.Add("综合评估为：垃圾短信");
        }
       
        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog file1 = new OpenFileDialog();
            file1.Filter = "文本文件|*.txt";
            if (file1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(file1.FileName, Encoding.Default);
                while (sr.EndOfStream != true)
                {                  
                    textBox1.Text = sr.ReadLine();
                }
            }
        }

        public void button9_Click(object sender, EventArgs e)
        {
            Form2 a = new Form2();
            a.Show(this);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //构造name数组，存储每一个word
            string[] stringsdd = File.ReadAllLines(@"..\Debug\textgather\gather\collecto.txt", Encoding.Default);
            string[] name = new string[stringsdd.Length];
            int x = 0;
            //构造iavg数组，存储每一个word的文档频度
            double[] iavg = new double[stringsdd.Length];
            //遍历collecto.txt文档取特征词
            foreach (string words in File.ReadAllLines(@"..\Debug\textgather\gather\collecto.txt", Encoding.Default))
            {
                //将words中第一个词提取出来作为word
                int j = words.IndexOf(" ");
                String word = words.Substring(0, j);
                name[x] = word;
                //在collect中统计包涵word的总文档数，用i记录
                int i = 1;
                foreach (string strs in File.ReadAllLines(@"..\Debug\textgather\gather\collect.txt", Encoding.Default))
                {
                    if (strs.IndexOf(word) > 0) i++;
                }
                //iavg为总的互信息
                iavg[x++] = i;
            }
            //共同排序
            Array.Sort(iavg, name);
            //反转使数组从大到小排列
            Array.Reverse(iavg);
            Array.Reverse(name);
            //写入collectok文档
            File.Create(@"..\Debug\textgather\gather\collectok.txt").Close();
            StreamWriter swt = new StreamWriter(@"..\Debug\textgather\gather\collectok.txt", true, Encoding.Default);
            for (x = 0; x < 3200; x++)
            {
                swt.WriteLine(name[x] + "   " + iavg[x].ToString());
            }
            swt.Close();
            MessageBox.Show("成功");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //计算三种类型各有多少样本,以及总样本
            string[] files = Directory.GetFiles(@"..\Debug\textgather\normal\");
            int normalnum = files.Length;
            files = Directory.GetFiles(@"..\Debug\textgather\inform\");
            int informnum = files.Length;
            files = Directory.GetFiles(@"..\Debug\textgather\spam\");
            int spamnum = files.Length;
            int allnum = normalnum + informnum + spamnum;
            //构造name数组，存储每一个word
            string[] stringsdd = File.ReadAllLines(@"..\Debug\textgather\gather\collecto.txt", Encoding.Default);
            string[] name = new string[stringsdd.Length];
            int x = 0;
            //构造iavg数组，存储每一个word的互信息
            double[] imax = new double[stringsdd.Length];
            //遍历collecto.txt文档取特征词
            foreach (string words in File.ReadAllLines(@"..\Debug\textgather\gather\collecto.txt", Encoding.Default))
            {
                //将words中第一个词提取出来作为word
                int j = words.IndexOf(" ");
                String word = words.Substring(0, j);
                name[x] = word;
                //在collect中统计包涵word的总文档数，用i记录
                int i = 1;
                foreach (string strs in File.ReadAllLines(@"..\Debug\textgather\gather\collect.txt", Encoding.Default))
                {
                    if (strs.IndexOf(word) > 0) i++;
                }
                //k记录当前行
                int k = 0;
                //计算word出现在各类的频率
                int jnormal = 1;
                int jinform = 1;
                int jspam = 1;
                foreach (string strs in File.ReadAllLines(@"..\Debug\textgather\gather\collect.txt", Encoding.Default))
                {
                    k++;
                    if (k <= spamnum)
                    { if (strs.IndexOf(word) > 0) jspam++; }
                    else if (k <= spamnum+informnum)
                    { if (strs.IndexOf(word) > 0) jinform++; }
                    else
                    { if (strs.IndexOf(word) > 0) jnormal++; }
                }
                //计算每一类的互信息
                double i1, i2, i3;
                i1 = Math.Log((double)jnormal * allnum / ((double)(jnormal + i) * (jnormal + normalnum)));
                i2 = Math.Log((double)jinform * allnum / ((double)(jinform + i) * (jinform + informnum)));
                i3 = Math.Log((double)jspam * allnum / ((double)(jinform + i) * (jinform + spamnum)));
                i1 = Math.Abs(i1);
                i2 = Math.Abs(i2);
                i3 = Math.Abs(i3);
                //imax为总的互信息
                if (i1 > i2 && i1 > i3) imax[x++] = i1;
                else if (i2 > i1 && i2 > i3) imax[x++] = i2;
                else imax[x++] = i3;
            }
            //共同排序
            Array.Sort(imax, name);
            //反转使数组从大到小排列
            Array.Reverse(imax);
            Array.Reverse(name);
            //写入collectok文档
            File.Create(@"..\Debug\textgather\gather\collectok.txt").Close();
            StreamWriter swt = new StreamWriter(@"..\Debug\textgather\gather\collectok.txt",true, Encoding.Default);
            for (x = 0; x < 2300; x++)
            {
                swt.WriteLine(name[x] + "   " + imax[x].ToString());
            }
            swt.Close();
            MessageBox.Show("成功");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //计算三种类型各有多少样本,以及总样本
            string[] files = Directory.GetFiles(@"..\Debug\textgather\normal\");
            int normalnum = files.Length;
            files = Directory.GetFiles(@"..\Debug\textgather\inform\");
            int informnum = files.Length;
            files = Directory.GetFiles(@"..\Debug\textgather\spam\");
            int spamnum = files.Length;
            int allnum = normalnum + informnum + spamnum;
            //构造name数组，存储每一个word
            string[] stringsdd = File.ReadAllLines(@"..\Debug\textgather\gather\collecto.txt", Encoding.Default);
            string[] name = new string[stringsdd.Length];
            int x = 0;
            //构造iavg数组，存储每一个word的卡方统计量
            double[] imax = new double[stringsdd.Length];
            //遍历collecto.txt文档取特征词
            foreach (string words in File.ReadAllLines(@"..\Debug\textgather\gather\collecto.txt", Encoding.Default))
            {
                //将words中第一个词提取出来作为word
                int j = words.IndexOf(" ");
                String word = words.Substring(0, j);
                name[x] = word;
                //在collect中包涵word的总文档数，用i表示
                int i = 0;
                foreach (string strs in File.ReadAllLines(@"..\Debug\textgather\gather\collect.txt", Encoding.Default))
                {
                    if (strs.IndexOf(word) > 0) i++;
                }
                //k记录当前行
                int k = 0;
                //计算word出现在各类的频次
                int jnormal = 0;
                int jinform = 0;
                int jspam = 0;
                foreach (string strs in File.ReadAllLines(@"..\Debug\textgather\gather\collect.txt", Encoding.Default))
                {
                    k++;
                    if (k <= spamnum)
                    { if (strs.IndexOf(word) > 0) jspam++; }
                    else if (k <= spamnum + informnum)
                    { if (strs.IndexOf(word) > 0) jinform++; }
                    else
                    { if (strs.IndexOf(word) > 0) jnormal++; }
                }
                //计算A、B、C、D四个数方便下一步计算
                int a1 = jnormal;
                int b1 = i-jnormal;
                int c1 = normalnum-jnormal;
                int d1 = allnum-a1-b1-c1;

                int a2 = jinform;
                int b2 = i-jinform;
                int c2 = informnum - jinform;
                int d2 = allnum - a2 - b2 - c2;

                int a3 = jspam;
                int b3 = i-jspam;
                int c3 = spamnum - jspam;
                int d3 = allnum - a3 - b3 - c3;
                //计算每一类的卡方统计量
                double i1, i2, i3;
                i1 = Math.Pow(a1 * d1 - b1 * c1, 2) / ((a1 + b1) * (c1 + d1) * (a1 + c1) * (b1 + d1));
                i2 = Math.Pow(a2 * d2 - b2 * c2, 2) / ((a2 + b2) * (c2 + d2) * (a2 + c2) * (b2 + d2));
                i3 = Math.Pow(a3 * d3 - b3 * c3, 2) / ((a3 + b3) * (c3 + d3) * (a3 + c3) * (b3 + d3));
                //imax为总的卡方统计量
                if (i1 > i2 && i1 > i3) imax[x++] = i1;
                else if (i2 > i1 && i2 > i3) imax[x++] = i2;
                else imax[x++] = i3;
            }
            //共同排序
            Array.Sort(imax, name);
            //反转使数组从大到小排列
            Array.Reverse(imax);
            Array.Reverse(name);
            //写入collectok文档
            File.Create(@"..\Debug\textgather\gather\collectok.txt").Close();
            StreamWriter swt = new StreamWriter(@"..\Debug\textgather\gather\collectok.txt", true, Encoding.Default);
            for (x = 0; x < 3200; x++)
            {
                swt.WriteLine(name[x] + "   " + imax[x].ToString());
            }
            swt.Close();
            MessageBox.Show("成功");
        }
    }
}
