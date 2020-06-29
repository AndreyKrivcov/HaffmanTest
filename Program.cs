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
            string str = "Mama mila ramu"; 
            char[] arr = str.ToCharArray();
            BitArray ba=new BitArray(new System.Text.ASCIIEncoding().GetBytes(str));

            Haffman<char> haffman = new Haffman<char>();

            var data = haffman.Encode(arr);

            Console.WriteLine("==============================");
            Console.WriteLine($"Inital = {ba.Length} bits");
            Console.WriteLine($"Haffman = {data.Value.Length} bits");
            Console.WriteLine("==============================");

            string s= new string(haffman.Decode(data.Key, data.Value));

            Console.WriteLine(s);
        }
    }


}
