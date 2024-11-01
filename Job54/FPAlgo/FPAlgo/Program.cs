using System;
using System.Collections.Generic;
using System.Text;

namespace FPAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Example usage of the new function
            List<string> transactions1 = new List<string>
            {
                "f a c d g i m p",
                "a b c f l m o",
                "b f h j o",
                "b c k s p",
                "a f c e l p m n",
            };
            int support = 2;
            var fpGrowth = new FPGrowth(transactions1, support);

            // Tìm kiếm và in các tập mục thường xuyên
            var frequentItemsets = fpGrowth.GetFrequentItemsets();

            // In tập phổ biến
            Helper.PrintFrequentItemsets(frequentItemsets, transactions1, support);

            //// Tạo và in luật kết hợp
            var rules = Helper.GetAssociationRules(frequentItemsets, transactions1, support);
            Helper.PrintAssociationRules(rules);


            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

        }



    }


}
