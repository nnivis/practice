using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FirstStream
{
    internal class Program
    {
        // Класс FileStream. 1) Ввод в файла. 2) Чтение из файла. 3) С помощью метода Seek найдём последние 6 символом и заменим их другими.
        static void Main(string[] args)
        {
            Console.WriteLine("Ввидите строку для записи в файл");
            //string text = Console.ReadLine();
            string text = "Hello, World!";

            using (FileStream fstream = new FileStream("input_one.txt", FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст был записан в файл");
            }

            using (FileStream fstream = File.OpenRead("input_one.txt"))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                string textFromFile = Encoding.Default.GetString(array); 
                Console.WriteLine("Текст был выведен файла: " + textFromFile);
            }


            using (FileStream fstream = new FileStream("input_one.txt", FileMode.OpenOrCreate))
            {
                byte[] input = Encoding.Default.GetBytes(text);
                fstream.Write(input, 0, input.Length);
                Console.WriteLine("Текст был записан в файл");

                fstream.Seek(-6, SeekOrigin.End);

                byte[] output = new byte[6];
                fstream.Read(output, 0, output.Length);
                string textFromFile = Encoding.Default.GetString(output);
                Console.WriteLine("Заменим данное слово: " + textFromFile);

                string changeText = "House!";
                fstream.Seek(-6, SeekOrigin.End);
                input = Encoding.Default.GetBytes(changeText);
                fstream.Write(input, 0, input.Length);
                fstream.Seek(0, SeekOrigin.Begin);
                output = new byte[fstream.Length];
                fstream.Read(output,0, output.Length);
                textFromFile = Encoding.Default.GetString(output);
                Console.WriteLine("Текст был выведен файла: " + textFromFile);
             
            }

                Console.Read();
        }    
    }
}
