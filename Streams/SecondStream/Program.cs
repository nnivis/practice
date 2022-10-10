using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SecondStream
{
    internal class Program
    {
        // Класс StreamReader. 1) Считываем весь файл с помощью ReadToEnd. 2) Считываем файл построчно с помощью ReadLine. 3) Считываем определённое число символов.
        static void Main(string[] args)
        {
            string text = "input_one.txt";

            try
            {
                Console.WriteLine("Считывается весь файл");
                using (StreamReader sr = new StreamReader(text))
                {
                    Console.WriteLine(sr.ReadToEnd());
                }

                Console.WriteLine("Считывается построчно\n");
                using (StreamReader sr = new StreamReader(text))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }

                Console.WriteLine("Считывается определенное число символов\n");
                using (StreamReader sr = new StreamReader(text))
                {
                    int num = 10;
                    char[] array = new char[num];
                    sr.Read(array, 0, num);
                    Console.WriteLine(array);
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
