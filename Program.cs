using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DronTropy
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] arrayPath = new string[3] { @"D:\Text\OneText", @"D:\Text\TwoText", @"D:\Text\ThreeText" };
            FileInfo file = null;
            List<string> result = new List<string>();
            string[] arrayBASE64 = new string[3];
            var listEnt = new List<List<Entropiy>>();
            List<Entropiy> oneText = new List<Entropiy>();
            List<Entropiy> twoText = new List<Entropiy>();
            List<Entropiy> threeText = new List<Entropiy>();
            listEnt.Add(oneText);
            listEnt.Add(twoText);
            listEnt.Add(threeText);
            for(var index=0; index < 3; index++)
            {
                listEnt[index].Add(new Entropiy(arrayPath[index]+".txt", true));
                listEnt[index].Add(new Entropiy(MyBase64.GetBase64(arrayPath[index] + ".txt")));
                listEnt[index].Add(new Entropiy(MyBase64.GetBase64(arrayPath[index] + ".7z")));
            }
            int count = 0;
            foreach(var elem in listEnt)
            {
                foreach(var ent in elem)
                {
                    result.Add(ent.Path);
                    result.AddRange(ent.GetProbality());
                    result.Add("Ентропія:" + ent.Entropiya.ToString());
                    result.Add("Кількість інформації байтах :" + ent.AmountInformation + "  в битах:" + ent.AmountInformation / 8);
                    result.Add("Дліна тексту" + ent.Text.Length);                   
                    count++;
                }
                count = 0;
                result.Add("-------------------");
            }

            foreach (var el in arrayPath)
            {
                
                file = new FileInfo(el + ".txt");
                result.Add(el + " " + file.Length/8);
            }
            foreach (var el in arrayPath)
            {
                file = new FileInfo(el + ".7z");
                result.Add(el + " " + file.Length / 8);
            }
            result.Add("OneText.txt: " + MyBase64.GetBase64(@"D:\Text\OneText.txt"));
            result.Add("TwoText.txt: " + MyBase64.GetBase64(@"D:\Text\TwoText.txt"));
            result.Add("ThreeText.txt: " + MyBase64.GetBase64(@"D:\Text\ThreeText.txt"));
            result.Add("-------");
            result.Add("OneText.7z: " + MyBase64.GetBase64(@"D:\Text\TwoText.7z"));
            result.Add("TwoText.7z: " + MyBase64.GetBase64(@"D:\Text\TwoText.7z"));
            result.Add("Three.7z: " + MyBase64.GetBase64(@"D:\Text\ThreeText.7z"));
            using (StreamWriter sw = new StreamWriter(@"C:\log.txt", true))
            {
                foreach (var el in result)
                {
                    sw.WriteLine(el);
                }
            }
            Console.WriteLine(MyBase64.GetBase64(@"D:\Text\TwoText.txt"));
     
           
            Console.ReadKey();
        }
    }
    public
    class Entropiy
    {
        public Dictionary<char, double> charProbability = null;
        public string Text { get; set; }

        public double Entropiya
        {
            get;
            private set;
        }
        public List<string> GetProbality()
        {
            var result = new List<string>();
            foreach (char key in charProbability.Keys.ToArray())
            {
             
                result.Add("Symbol " + key + " " + charProbability[key].ToString());
            }
            return result;
        }
        public double AmountInformation
        {
            get
            {
                return this.Text.Length * this.Entropiya;
            }
        }

        public string Path { get; set; }
        public Entropiy(string path, bool isFile = false)
        {
         
            if (isFile)
            {
                this.Path = path;
                using (StreamReader sr = new StreamReader(path))
                {
                    Text = sr.ReadToEnd();
                }
            }
            else
            {
                this.Path = "Base64 or 7z";
                this.Text = path;
            }

            this.charProbability = new Dictionary<char, double>();

            foreach (var el in Text)//get count every char in text
            {
                if (!charProbability.ContainsKey(el))
                {
                    charProbability.Add(el, 1);
                }
                else
                {
                    charProbability[el] = charProbability[el] + 1;
                }
            }

            int textLen = Text.Length;
            foreach (char key in charProbability.Keys.ToArray())
            {
                charProbability[key] = charProbability[key] / textLen;//get probability as (count this char in text) / (text len)
            }

            foreach (double val in charProbability.Values)
            {
                Entropiya += val * Math.Log(1 /val, 2);
            }        
        }
    }
}
