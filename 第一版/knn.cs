using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第一版
{
    class knn
    {
        private double norchance;
        private double infchance;
        private double spachance;
        //用mytext数组存储其文本向量
        private int[] mytext=new int[5000];
        private int i = 0;
        public void Compute(string content)
        {
            //遍历collectok.txt文档取特征词
            foreach (string words in File.ReadAllLines(@"..\Debug\textgather\gather\collectok.txt", Encoding.Default))
            {
                //将words中第一个词提取出来作为word
                int j = words.IndexOf(" ");
                String word = words.Substring(0, j);
                //查找word在content中的出现次数
                string s1 = content.Replace(word, "");
                int number = (content.Length - s1.Length) / word.Length;
                mytext[i] = number;
                i++;
            }
            //计算三种类型各有多少样本,以及总样本
            string[] files = Directory.GetFiles(@"..\Debug\textgather\normal\");
            int normalnum = files.Length;
            files = Directory.GetFiles(@"..\Debug\textgather\inform\");
            int informnum = files.Length;
            files = Directory.GetFiles(@"..\Debug\textgather\spam\");
            int spamnum = files.Length;
            int allnum = normalnum + informnum + spamnum;
            //构造类名数组
            int[] name = new int[allnum];
            for (int sc = 0; sc < allnum; sc++)
            {
                if (sc < spamnum) name[sc] = 1;
                else if (sc < spamnum + informnum) name[sc] = 2;
                else name[sc] = 3;
            }
            //计算欧氏距离并存入range数组
            double[] range = new double[allnum];
            i = 0;
            foreach (string words in File.ReadAllLines(@"..\Debug\textgather\gather\trainknn.txt", Encoding.Default))
            {
                //求平方和
                int all = 0;
                string[] sArray = words.Split(' ');
                int k = 0;
                foreach (string s in sArray)
                {
                    //因为split将words切割后，最后一个字符为空字符，用if将其去掉
                    if (s.Length <= 0) break;
                    all = all + (int)Math.Pow(mytext[k++] - int.Parse(s), 2);
                }
                //开方并存入数组
                range[i++] = Math.Sqrt(all);
            }
            //共同排序
            Array.Sort(range, name);
            int a = 0, b = 0, c = 0;
            for (i = 0; i < 1; i++)
            {
                //统计概率
                if (name[i] == 1) a++;
                else if (name[i] == 2) b++;
                else c++;
            }
            norchance = (double)c / (a + b + c);
            infchance = (double)b / (a + b + c);
            spachance = (double)a / (a + b + c);
            norchance = Math.Round(norchance, 4);
            infchance= Math.Round(infchance, 4);
            spachance= Math.Round(spachance, 4);
        }
        public double getnorchance
        {
            get { return norchance; }
        }
        public double getinfchance
        {
            get { return infchance; }
        }
        public double getspachance
        {
            get { return spachance; }
        }

    }
}
