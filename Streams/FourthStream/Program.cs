using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FourthStream
{
    internal class Program
    {
        // Класс MemoryStream. 1) Записываем содержимое файла, программа будет считывать каждый байт, который она нашла в MemoryStream, и выводить на консоль.
        static void Main(string[] args)
        {
			
			byte[] textOne = File.ReadAllBytes("input_one.txt");
			using (MemoryStream memoryStream = new MemoryStream(textOne))
			{
				int b;
				while ((b = memoryStream.ReadByte()) >= 0)
					Console.WriteLine(Convert.ToChar(b));
			}

			Console.Read();
		}
    }
}
