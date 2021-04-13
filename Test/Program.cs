using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            double n = 6.255;
            Console.WriteLine(Math.Round(n,2,MidpointRounding.ToEven));
        }
    }
}
