using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            int a, b;
            Random random = new Random();
            int operation;
           
            int result;
            string resultuserstring = "";
            int resultuser;

            bool run = true;
            int i = 0;

            Console.WriteLine("Hello! Do you know an answer?");

            while (run)
            {
                operation = random.Next(0, 2);


                switch (operation)
                {
                    case 0:
                        
                        
                        a = random.Next(0, 99);
                        b = random.Next(0, 99);
                        Console.WriteLine("{0} + {1} = ?", a, b);
                        resultuserstring = Console.ReadLine();
                        resultuser = int.Parse(resultuserstring);
                        result = a + b;
                        if (resultuser == result)
                        {
                            
                            run = true;
                            i++;
                            Console.WriteLine("correct!");
                            Console.Clear();
                            Console.WriteLine("Next: ");
                        }
                        else
                        {
                            Console.Write("incorrect :( \n Total correct answers: {0}", i);
                            run = false;
                            Console.ReadKey();
                        }
                        break;

                    case 1:

                        a = random.Next(0, 99);
                        b = random.Next(0, 99);
                        Console.WriteLine("{0} - {1} = ?", a, b);
                        resultuserstring = Console.ReadLine();
                        resultuser = int.Parse(resultuserstring);
                        result = a - b;
                        if (resultuser == result)
                        {
                            run = true;
                            i++;
                            Console.WriteLine("correct!");
                            Console.Clear();
                            Console.WriteLine("Next: ");
                        }
                        else
                        {
                            Console.Write("incorrect :( \n Total correct answers: {0}", i);
                            run = false;
                            Console.ReadKey();
                        }
                        break;      
                }
            }
        }
    }
}
