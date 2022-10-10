using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FifthStream
{
    // Класс NetworkStream.
    class TcpDemo
    {
        static void Main()
        {
            new Thread(Server).Start();
            Thread.Sleep(500);
            Client();

            Console.ReadLine();
        }
        static void Client()
        {
            using (TcpClient client = new TcpClient("localhost", 51111))
            {
                using (NetworkStream n = client.GetStream())
                {
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write("Hello");
                    w.Flush();
                    Console.WriteLine(new BinaryReader(n).ReadString());
                }
            }
        }
        static void Server()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 51111);
            listener.Start();
            using (TcpClient client = listener.AcceptTcpClient())
            {
                using (NetworkStream n = client.GetStream())
                {
                    string msg = new BinaryReader(n).ReadString();
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write(msg + " World!");
                    w.Flush();
                }
            }

            listener.Stop();
        }
    }
}
