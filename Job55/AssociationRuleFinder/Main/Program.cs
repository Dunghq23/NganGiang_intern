using Algorithms.Apriori;
using Algorithms.FPGrowth;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("Nhập số lượng giao dịch muốn lấy từ CSDL (hoặc nhập 0 để sử dụng giao dịch mẫu): ");
            if (!int.TryParse(Console.ReadLine(), out int transactionCount))
            {
                Console.WriteLine("Vui lòng nhập một số hợp lệ.");
                return;
            }

            List<string> transactions = new List<string>();
            if (transactionCount > 0)
            {
                transactions = FetchTransactionsFromDatabase(transactionCount);
            }
            else
            {
                Console.WriteLine("Sử dụng giao dịch mẫu");
                transactions = new List<string>
                {
                    "f a c d g i m p",
                    "a b c f l m o",
                    "b f h j o",
                    "b c k s p",
                    "a f c e l p m n",
                };
            }

            Console.Write($"Ngưỡng hỗ trợ tối thiểu (min = 2): ");
            if (!int.TryParse(Console.ReadLine(), out int supportThreshold) || supportThreshold < 2)
            {
                Console.WriteLine("Vui lòng nhập một ngưỡng hỗ trợ tối thiểu hợp lệ (tối thiểu là 2).");
                return;
            }

            var transactionsPreprocessed = Helper.PreprocessDatabase(transactions);

            string choice = GetUserChoice();
            if (choice == "3") return; // Thoát chương trình nếu chọn 3

            RunAlgorithm(choice, transactionsPreprocessed, supportThreshold);
        }

        static List<string> FetchTransactionsFromDatabase(int transactionCount)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> transactions = new List<string>();
            try
            {
                string query = $"SELECT TOP {transactionCount} Trans FROM Transactions";
                DataTable result = DatabaseHelper.ExecuteQuery(query);
                foreach (DataRow row in result.Rows)
                {
                    string transaction = row["Trans"].ToString();
                    transactions.Add(transaction);
                }
                //Console.WriteLine("Danh sách giao dịch lấy từ cơ sở dữ liệu:");
                //transactions.ForEach(Console.WriteLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi xảy ra khi kết nối CSDL: " + ex.Message);
            }

            stopwatch.Stop();
            Console.WriteLine($"\n\nThời gian lấy dữ liệu: {stopwatch.Elapsed}");

            return transactions;
        }

        static string GetUserChoice()
        {
            //Console.Clear();
            Console.WriteLine("Chọn thuật toán để chạy:");
            Console.WriteLine("1. FP-Growth");
            Console.WriteLine("2. Apriori");
            Console.WriteLine("3. Thoát chương trình");
            Console.Write("Nhập lựa chọn của bạn (1, 2, hoặc 3): ");
            return Console.ReadLine();
        }

        static void RunAlgorithm(string choice, List<string> transactions, int supportThreshold)
        {
            List<ItemSet> frequentItemSets = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nChạy thuật toán FP-Growth...");
                    frequentItemSets = new FPGrowth(transactions, supportThreshold).GetFrequentItemsets();
                    break;

                case "2":
                    Console.WriteLine("\nChạy thuật toán Apriori...");
                    frequentItemSets = new Apriori(transactions, supportThreshold).GetFrequentItemSets();
                    break;

                default:
                    Console.WriteLine("Lựa chọn không hợp lệ. Kết thúc chương trình.");
                    return;
            }

            Helper.PrintFrequentItemsets(frequentItemSets, supportThreshold);
            List<AssociationRule> rules = Helper.GetAssociationRules(frequentItemSets, transactions.Count, supportThreshold);
            Helper.PrintAssociationRule(rules);

            stopwatch.Stop();
            Console.WriteLine($"\n\nThời gian thực thi: {stopwatch.Elapsed}");

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }
    }
}
