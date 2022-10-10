using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ThirdStream
{
    internal class Program
    {
        // Класс StreamWriter. 1) Считывает текст из первого файла, записывает его во второй файл, а также записывает дополнение.
        static void Main(string[] args)
        {
            string readText = "input_one.txt";
            string writeText = "input_two.txt";
            string text = "";

            try
            {
                using (StreamReader sr = new StreamReader(readText))
                {
                    text = sr.ReadToEnd();
                }

                using (StreamWriter sw = new StreamWriter(writeText, false))
                {
                    sw.WriteLine(text);
                }

                using (StreamWriter sw = new StreamWriter(writeText, true))
                {
                    sw.WriteLine(1);
                    sw.WriteLine("Если Вы опустите руки");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
