using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace TestHaffman
{
    class Program
    {
        static void Main(string[] args)
        {
            var encoding = new System.Text.ASCIIEncoding();

            BitArray arr = new BitArray(new bool[] {true, false, false, true, true, true, false, true, false, true, false, false, true, true, true, false, true});
            PrintBA(arr);
            string s = arr.GetString(encoding);
            BitArray newArr = s.GetBitArray(encoding,arr.Length);
            PrintBA(newArr);


            s = "This is my test string";
            BitArray test_s = s.GetBitArray(encoding,s.Length*16);
            string restored_s = test_s.GetString(encoding);

            Console.WriteLine(s);
            Console.WriteLine(restored_s);

            /*string str = "Mama mila ramu"; 
            char[] arr = str.ToCharArray();
            BitArray ba=new BitArray(new System.Text.ASCIIEncoding().GetBytes(str));

            Haffman<char> haffman = new Haffman<char>();

            var data = haffman.Encode(arr);

            Console.WriteLine("==============================");
            Console.WriteLine($"Inital = {ba.Length} bits");
            Console.WriteLine($"Haffman = {data.Value.Length} bits");
            Console.WriteLine("==============================");

            var encoding = new System.Text.ASCIIEncoding();



            PrintBA(data.Value);
            BitArray newBA = data.Value.GetString(encoding).GetBitArray(encoding,39);
            PrintBA(newBA);

            string s= new string(haffman.Decode(data.Key, 
            data.Value.GetString(new System.Text.ASCIIEncoding()).GetBitArray(new System.Text.ASCIIEncoding(),38)));

            Console.WriteLine($"{data.Value.GetString(new System.Text.ASCIIEncoding())} = {s}");*/
        }

        static void PrintBA(BitArray ba)
        {
            foreach (bool item in ba)
            {
                Console.Write((item ? "1" : "0"));
            }
            Console.WriteLine();
        }
    }

}
