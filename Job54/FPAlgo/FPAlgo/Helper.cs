using System;
using System.Collections.Generic;
using System.Linq;

namespace FPAlgo
{
    public static class Helper
    {
        // Tiền xử lý cơ sở dữ liệu: Loại bỏ khoảng trắng và sử dụng dấu phẩy để phân tách các mục trong giao dịch
        public static List<string> PreprocessDatabase(List<string> database)
        {
            return database.Select(transaction => string.Join(",", transaction.Split(' ').Select(item => item.Trim()))).ToList();
        }

        // Sinh các tổ hợp k phần tử từ một danh sách
        public static List<List<T>> GetCombinations<T>(List<T> list, int k)
        {
            var result = new List<List<T>>();
            GetCombinations(list, new List<T>(), k, 0, result);
            return result;
        }

        // Hàm đệ quy sinh các tổ hợp
        private static void GetCombinations<T>(List<T> list, List<T> tempList, int k, int start, List<List<T>> result)
        {
            if (tempList.Count == k)
            {
                result.Add(new List<T>(tempList));
                return;
            }

            for (int i = start; i < list.Count; i++)
            {
                tempList.Add(list[i]);
                GetCombinations(list, tempList, k, i + 1, result);
                tempList.RemoveAt(tempList.Count - 1);
            }
        }

        // Đếm số lần xuất hiện của một tập mục trong cơ sở dữ liệu
        public static int CountItemset(List<string> itemset, List<string> database)
        {
            return database.Count(transaction => itemset.All(item => transaction.Split(',').Select(i => i.Trim()).Contains(item)));
        }

        // Sinh luật kết hợp từ tập phổ biến
        public static List<AssociationRule> GetAssociationRules(List<List<string>> frequentItemsets, List<string> database, int minSup)
        {
            database = PreprocessDatabase(database);
            var rules = new List<AssociationRule>();

            foreach (var itemset in frequentItemsets)
            {
                // Sinh các tập con của tập phổ biến để tạo luật kết hợp
                for (int k = 1; k < itemset.Count; k++)
                {
                    var combinations = GetCombinations(itemset, k);
                    foreach (var combination in combinations)
                    {
                        var consequent = itemset.Except(combination).ToList();
                        var rule = new AssociationRule
                        {
                            Label = $"{string.Join(", ", combination)} => {string.Join(", ", consequent)}"
                        };

                        // Tính Support
                        double support = (double)CountItemset(itemset, database) / database.Count;
                        if (support >= (double)minSup / database.Count)
                        {
                            rule.Support = Math.Round(support * 100, 2);

                            // Tính Confidence
                            int combinationCount = CountItemset(combination, database);
                            rule.Confidence = combinationCount == 0 ? 0 : Math.Round((double)CountItemset(itemset, database) * 100 / combinationCount, 2);

                            rules.Add(rule);
                        }
                    }
                }
            }

            // Sắp xếp luật theo Support và Confidence
            return rules.OrderByDescending(r => r.Support).ThenByDescending(r => r.Confidence).ToList();
        }

        // In tập mục phổ biến
        public static void PrintFrequentItemsets(List<List<string>> frequentItemsets, List<string> database, int minSup)
        {
            database = PreprocessDatabase(database);
            var groupedByLength = frequentItemsets
                .GroupBy(itemset => itemset.Count)
                .OrderBy(g => g.Key)
                .ToList();

            foreach (var group in groupedByLength)
            {
                Console.WriteLine($"\nFrequent ItemSet (Length = {group.Key}, Support >= {minSup}):");
                var sortedItemsets = group
                    .Select(itemset => itemset.OrderBy(item => item).ToList())
                    .OrderBy(itemset => string.Join(",", itemset))
                    .ToList();

                foreach (var itemset in sortedItemsets)
                {
                    int count = CountItemset(itemset, database);
                    // In ra tập phổ biến kèm theo số lần xuất hiện
                    Console.WriteLine($"Item: {string.Join(", ", itemset)}, Count: {count}");
                }
            }
        }

        // In các luật kết hợp
        public static void PrintAssociationRules(List<AssociationRule> rules)
        {

            Console.WriteLine("\nAssociation Rules:");
            foreach (var rule in rules)
            {
                Console.WriteLine($"Rule: {rule.Label}, Support: {rule.Support}%, Confidence: {rule.Confidence}%");
            }
        }
    }
}
