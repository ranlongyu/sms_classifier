using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第一版
{
    class bayes
    {
        private double firstchance=1;
        private double secondchance=1;
        private double thirdchance=1;
        //遍历训练好的train文档的每一行，计算文本属于类一，类二，类三的总似然
        public void Compute(string content)
        {
            var segmenter = new JiebaSegmenter();
            var segments = segmenter.Cut(content);
            foreach (string strs in File.ReadAllLines(@"..\Debug\textgather\gather\train.txt", Encoding.Default))
            {
                //临时记录每一行的数据
                string word = "";
                float a = 0;
                float b = 0;
                float c = 0;
                string[] sArray = strs.Split(' ');
                foreach (string i in sArray)
                {
                    if (word == "") word = i;
                    else if (a == 0) a = float.Parse(i);
                    else if (b == 0) b = float.Parse(i);
                    else c = float.Parse(i);
                }
                //遍历待分类短信，查看是否有相同的词
                //设置singal看是否在待分类短信中找到了该词
                int singal = 0;
                foreach (string str in segments)
                {
                    if (word == str)
                    {
                        singal = 1;
                        firstchance = a * firstchance;
                        secondchance = b * secondchance;
                        thirdchance = c * thirdchance;
                        break;
                    }
                }
                if (singal == 1)
                {
                    firstchance = (1 - a) * firstchance;
                    secondchance = (1 - b) * secondchance;
                    thirdchance = (1 - c) * thirdchance;
                }
            }
            //用短信的长度代替先验概率
            if (content.Length < 8)
            {
                //计算总似然
                firstchance = firstchance * (float)0.7;
                secondchance = secondchance * (float)0.15;
                thirdchance = thirdchance * (float)0.15;
            }
            else
            {
                //计算总似然
                firstchance = firstchance * (float)0.3;
                secondchance = secondchance * (float)0.35;
                thirdchance = thirdchance * (float)0.35;
            }
            //外加网址检测做为先验概率
            if (content.Contains("http"))
            {
                //计算总似然
                firstchance = firstchance * (float)0.1;
                secondchance = secondchance * (float)0.45;
                thirdchance = thirdchance * (float)0.45;
            }
            double sum = firstchance + secondchance + thirdchance;
            firstchance = Math.Round(firstchance / sum, 4);
            secondchance = Math.Round(secondchance / sum, 4);
            thirdchance = Math.Round(thirdchance / sum, 4);
        }

        public double getnorchance
        {
            get{return firstchance;}
        }
        public double getinfchance
        {
            get { return secondchance; }
        }
        public double getspachance
        {
            get { return thirdchance; }
        }
    }
}
