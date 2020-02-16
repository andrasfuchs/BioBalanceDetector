using System;

namespace Mars_64
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            Console.Write("Waiting for debugger to attach... ");
            while (true)
            {
                Console.Write(++i + " ");
                if (System.Diagnostics.Debugger.IsAttached) break; 
                System.Threading.Thread.Sleep(1000); 
            }
            Console.WriteLine();
        }
    }
}
