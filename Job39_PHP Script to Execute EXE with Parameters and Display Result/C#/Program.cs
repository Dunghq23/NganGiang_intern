using System;
using System.IO;

class SumProgram
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: sum.exe <num1> <num2>");
            return;
        }

        int num1 = Int32.Parse(args[0]);
        int num2 = Int32.Parse(args[1]);
        int sum = num1 + num2;

        File.WriteAllText("result.txt", sum.ToString());
    }
}
