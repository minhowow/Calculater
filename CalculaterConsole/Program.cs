using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculater;

namespace CalculaterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Block a = Block.Parse("sin(1)");
            Block b = Block.Parse("-1");
            Block c = a.Plus(b);

            Equation equation = Equation.Parse("sin(1+2*3)=3");

            Console.WriteLine(c.ToString());
            Console.WriteLine(c.Reduce().ToString());
            Console.WriteLine(c.Calculate());

            Console.WriteLine(equation.ToString());


            Block block1 = Block.Parse("x");
            double z = block1.Calculate(2);

            while (true)
            {
                try
                {

                    Block block = Block.Parse(Console.ReadLine());
                    Console.WriteLine("식: " + block.ToString());
                    Console.WriteLine("간단식: " + block.Reduce().ToString());
                    Console.WriteLine("간단식 대입: " + block.Reduce(2).ToString());
                    Console.WriteLine("결과: " + block.Calculate(2));
                    Console.WriteLine(new string('-', 8));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
