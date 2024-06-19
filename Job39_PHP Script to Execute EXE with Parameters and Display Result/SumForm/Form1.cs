using System;
using System.IO;
using System.Windows.Forms;

namespace SumForm
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();

            // Kiểm tra và xử lý đối số
            if (args.Length == 2)
            {
                if (int.TryParse(args[0], out int num1) && int.TryParse(args[1], out int num2))
                {
                    int sum = num1 + num2;
                    try
                    {
                        File.WriteAllText("result.txt", sum.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to write to result.txt: " + ex.Message);
                    }
                    Application.Exit();
                }
                else
                {
                    Console.WriteLine("Invalid arguments. Please provide two integers.", "Error");
                }
            }
            else
            {
                Console.WriteLine("Please provide exactly two arguments.", "Error");
            }
            Application.Exit();
        }

    }
}
