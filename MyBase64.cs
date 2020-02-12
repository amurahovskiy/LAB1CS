using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DronTropy
{
    public static class MyBase64
    {
        private static List<int> listBits;
        private static Dictionary<int, char> keySymbol = new Dictionary<int, char>();
        private static string element = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        static MyBase64()
        {
            int key = 0;
            foreach (var el in element)
            {
                keySymbol.Add(key, el);
                key++;
            }
        }
        private static int TranFor10System(List<int> byts)
        {

            int decimalNumber = 0;
            for (int i = 0; i < byts.Count; i++)
            {
                if (byts[byts.Count - i - 1] == 0) continue;
                decimalNumber += (int)Math.Pow(2, i);
            }
            return decimalNumber;
        }
        private static List<int> FillBits(byte byt)
        {
            var result = new List<int>();
            var bits = new Stack<int>();
            for (var index = 0; index < 8; index++)
            {
                bits.Push(byt % 2);
                byt /= 2;
            }

            return bits.ToList();
        }
        public static string GetBase64(string pathFile)
        {
            listBits = new List<int>();
            var listByteFile = File.ReadAllBytes(pathFile).ToList();
            foreach (var byt in listByteFile)
            {
                listBits.AddRange(FillBits(byt));
            }
            listBits.AddRange(listBits.Count % 6 != 0 ? new int[(listBits.Count / 6 + 1) * 6 - listBits.Count].ToList() : new List<int>(0));

            List<int> resultNumber = new List<int>();
            var temp = new List<int>();
            int count = 0;
            string s = "";
            foreach (var el in listBits)
            {
                temp.Add(el);
                count++;
                if (count == 6)
                {
                    resultNumber.Add(TranFor10System(temp));
                    temp = new List<int>();
                    count = 0;
                }
            }

            string resultBase64 = null;
            foreach (var el in resultNumber)
            {
                resultBase64 += keySymbol[el];
            }

            var append = listByteFile.Count % 3 == 1 ? "==" : listByteFile.Count % 3 == 2 ? "=" : "";
            resultBase64 += append;
            return resultBase64;


        }
    }
}
