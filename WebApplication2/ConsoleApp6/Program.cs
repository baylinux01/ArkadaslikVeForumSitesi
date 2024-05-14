using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int total = 0;
            for(int i=0;i<=5;i++)
            {
                total += i;
            }
            Console.WriteLine(total);
        }
    }
}
