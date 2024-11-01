//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AprioriAlgorithm;
//using System.Text.RegularExpressions;
//using System.Diagnostics;
//using System.Text;

//namespace ConsoleApp
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            // Example usage of the new function
//            List<string> transactions = new List<string>
//            {
//                "119 061 041",
//"114 043 046 008
//"117 028 005 037",
//"095 072 074 032 060",

//            };
//            int support = 2; // Ngưỡng hỗ trợ tối thiểu

//            Stopwatch stopwatch = new Stopwatch();
//            stopwatch.Start();

//            RunApriori(transactions, support);

//            stopwatch.Stop();
//            // Đổi từ mili giây sang giây
//            double seconds = stopwatch.ElapsedMilliseconds / 1000;
//            Console.WriteLine($"Thoi gian thuc hien: {seconds} giay.");

//            // Kết thúc chương trình
//            Console.WriteLine("\nPress any key to exit...");
//            Console.ReadKey();
//        }

//        /// <summary>
//        /// Chạy thuật toán Apriori với danh sách giao dịch và ngưỡng hỗ trợ được chỉ định.
//        /// </summary>
//        /// <param name="transactions">Danh sách giao dịch (mỗi giao dịch là một chuỗi chứa các mục, cách nhau bằng dấu cách).</param>
//        /// <param name="support">Ngưỡng hỗ trợ tối thiểu cho các tập phổ biến.</param>
//        static void RunApriori(List<string> transactions, int support)
//        {
//            int k = 1;
//            List<ItemSet> itemSets = new List<ItemSet>();
//            bool next;

//            // Khởi tạo đối tượng Apriori với danh sách giao dịch
//            Apriori apriori = new Apriori(transactions);
//            do
//            {
//                next = false;

//                // Lấy tập mục phổ biến có độ dài k với ngưỡng hỗ trợ tối thiểu
//                var L = apriori.GetItemSet(k, support);
//                if (L.Count > 0)
//                {
//                    List<AssociationRule> rules = new List<AssociationRule>();

//                    // Tạo các luật kết hợp nếu k != 1
//                    if (k != 1)
//                    {
//                        rules = apriori.GetRules(L);
//                    }

//                    // Hiển thị tập mục phổ biến
//                    Console.WriteLine($"\nFrequent ItemSet (Length = {k}, Support >= {support}):");
//                    foreach (var item in L)
//                    {
//                        Console.WriteLine($"Item: {string.Join(", ", item.Key)}, Count: {item.Value}");
//                    }

//                    // Hiển thị các luật kết hợp
//                    if (rules.Count > 0)
//                    {
//                        Console.WriteLine("\nAssociation Rules:");
//                        foreach (var rule in rules)
//                        {
//                            Console.WriteLine($"Rule: {rule.Label}, Support: {rule.Support}%, Confidence: {rule.Confidance}%");
//                        }
//                    }

//                    next = true;
//                    k++;
//                    itemSets.Add(L);
//                    Console.WriteLine($"k = {k}, count = {itemSets.Count}");
//                }

//            } while (next);
//        }
//    }
//}

//namespace ConsoleApp
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;
//            // Khởi tạo danh sách giao dịch mẫu
//            List<string> transactions = new List<string>
//            {
//                "f a c d g i m p",
//                "a b c f l m o",
//                "b f h j o",
//                "b c k s p",
//                "a f c e l p m n",
//            };
//            int supportThreshold = 3; // Ngưỡng hỗ trợ tối thiểu

//            // Gọi hàm RunApriori để tìm tập mục phổ biến
//            List<List<string>> frequentItemSets = RunApriori(transactions, supportThreshold);

//            // Khởi tạo đối tượng Apriori và lấy các quy tắc kết hợp
//            Apriori apriori = new Apriori(transactions, supportThreshold);
//            var rules = apriori.GetAssociationRules(frequentItemSets, transactions);

//            // Hiển thị các quy tắc kết hợp
//            Console.WriteLine("\nQuy tắc kết hợp:");
//            foreach (var rule in rules)
//            {
//                Console.WriteLine($"Quy tắc: {rule.Label}, Hỗ trợ: {rule.Support}%, Độ tin cậy: {rule.Confidence}%");
//            }

//            // Kết thúc chương trình
//            Console.WriteLine("\nNhấn phím bất kỳ để thoát...");
//            Console.ReadKey();
//        }

//        static List<List<string>> RunApriori(List<string> transactions, int support)
//        {
//            int k = 1;
//            List<List<string>> frequentItemSets = new List<List<string>>();  // Danh sách lưu trữ tập mục phổ biến
//            bool hasNext = true; // Biến để kiểm tra xem còn tập mục phổ biến nào không

//            // Khởi tạo đối tượng Apriori với danh sách giao dịch
//            Apriori apriori = new Apriori(transactions, support);

//            // Vòng lặp để tìm tập mục phổ biến cho đến khi không còn tập mục nào
//            while (hasNext)
//            {
//                // Lấy tập mục phổ biến có độ dài k với ngưỡng hỗ trợ tối thiểu
//                var frequentItems = apriori.GetItemSet(k, support);
//                if (frequentItems.Count > 0)
//                {
//                    // Thêm tập mục phổ biến vào danh sách
//                    foreach (var item in frequentItems)
//                    {
//                        frequentItemSets.Add(item.Key);  // Lưu tập mục phổ biến
//                    }

//                    // Hiển thị tập mục phổ biến
//                    Console.WriteLine($"\nTập mục phổ biến (Độ dài = {k}, Hỗ trợ >= {support}):");
//                    foreach (var item in frequentItems)
//                    {
//                        Console.WriteLine($"Mục: {string.Join(", ", item.Key)}, Số lần: {item.Value}");
//                    }

//                    k++;
//                }
//                else
//                {
//                    hasNext = false; // Không còn tập mục phổ biến nào
//                }
//            }

//            return frequentItemSets; // Trả về tất cả các tập mục phổ biến
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using AprioriAlgorithm;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Example usage of the new function
            List<string> transactions = new List<string>
            {
               "f a c d g i m p",
                    "a b c f l m o",
                    "b f h j o",
                    "b c k s p",
                    "a f c e l p m n",
            };
            int support = 2; // Ngưỡng hỗ trợ tối thiểu

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            RunApriori(transactions, support);

            stopwatch.Stop();
            // Đổi từ mili giây sang giây
            double seconds = stopwatch.ElapsedMilliseconds / 1000;
            Console.WriteLine($"Thời gian thực hiện: {seconds} giây.");

            // Kết thúc chương trình
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Chạy thuật toán Apriori với danh sách giao dịch và ngưỡng hỗ trợ được chỉ định.
        /// </summary>
        /// <param name="transactions">Danh sách giao dịch (mỗi giao dịch là một chuỗi chứa các mục, cách nhau bằng dấu cách).</param>
        /// <param name="support">Ngưỡng hỗ trợ tối thiểu cho các tập phổ biến.</param>
        static void RunApriori(List<string> transactions, int support)
        {
            int k = 1;
            List<ItemSet> itemSets = new List<ItemSet>();
            bool next;

            // Khởi tạo đối tượng Apriori với danh sách giao dịch
            Apriori apriori = new Apriori(transactions);

            do
            {
                next = false;

                // Lấy tập mục phổ biến có độ dài k với ngưỡng hỗ trợ tối thiểu
                var L = apriori.GetItemSet(k, support);
                if (L.Count > 0)
                {
                    List<AssociationRule> rules = new List<AssociationRule>();

                    // Tạo các luật kết hợp nếu k != 1
                    if (k != 1)
                    {
                        rules = apriori.GetRules(L);
                    }

                    // Hiển thị tập mục phổ biến
                    Console.WriteLine($"\nFrequent ItemSet (Length = {k}, Support >= {support}):");
                    foreach (var item in L)
                    {
                        Console.WriteLine($"Item: {string.Join(", ", item.Key)}, Count: {item.Value}");
                    }

                    // Hiển thị các luật kết hợp
                    if (rules.Count > 0)
                    {
                        Console.WriteLine("\nAssociation Rules:");
                        foreach (var rule in rules)
                        {
                            Console.WriteLine($"Rule: {rule.Label}, Support: {rule.Support}%, Confidence: {rule.Confidence}%");
                        }
                    }

                    next = true;
                    k++;
                    itemSets.Add(L);
                }

            } while (next);
        }
    }
}

