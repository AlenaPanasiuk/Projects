using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app2
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = "dddd";
            double a = 1.23456789;
            float b = (float)1.23456789;

            int dsize = sizeof(decimal);
            int floatsize = sizeof(float);
            int doublesize = sizeof(ushort);

            Console.WriteLine(a);
            Console.ReadKey();
        }
    }
}
