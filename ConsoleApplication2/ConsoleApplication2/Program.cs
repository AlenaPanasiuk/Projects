using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
 
       

    class Program
    {
        static void Main(string[] args)
        {

            byte x = 6;
            byte y = 44;
            byte z = 113;
            Console.WriteLine("~{0} = {1}", x, (byte)(~x));
            Console.WriteLine("{0} | {1} = {2}", y, z, (byte)(y | z));
            Console.WriteLine("{0} & {1} = {2}", y, z, (byte)(y & z));
            Console.WriteLine("{0} ^ {1} = {2}", y, z, (byte)(y ^ z));
            Console.WriteLine("{0} << 1 = {1}", y, y << 1);
            Console.WriteLine("{0} << 1 = {1}", y, y >> 2);

            Console.ReadKey();
        }
       
    }
}
