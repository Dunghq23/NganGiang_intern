using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class Helper
    {
        // Tiền xử lý dữ liệu giao dịch: Chuẩn hóa và chuyển về chuỗi với dấu phân cách ","
        public static List<string> PreprocessDatabase(List<string> database)
        {
            return database.Select(transaction => string.Join(",", transaction.Split(' ').Select(item => item.Trim()))).ToList();
        }

        #region Tìm luật kết hợp
        // Lấy tất cả các tổ hợp (combinations) của một danh sách với số lượng phần tử k
        public static List<List<T>> GetCombinations<T>(List<T> list, int k)
        {
            var result = new List<List<T>>();
            GetCombinations(list, new List<T>(), k, 0, result);
            return result;
        }

        // Đệ quy để tạo tổ hợp
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

        // Đếm số lần xuất hiện của một tập hợp item trong danh sách giao dịch
        public static int CountItemset(List<string> itemset, List<string> transactions)
        {
            return transactions.Count(transaction => itemset.All(item => transaction.Split(',').Select(i => i.Trim()).Contains(item)));
        }

        // Tìm luật kết hợp từ các tập phổ biến
        //public static List<AssociationRule> GetAssociationRules(List<List<string>> frequentItemsets, List<string> transactions, int minSup)
        //{
        //    var rules = new List<AssociationRule>();

        //    foreach (var itemset in frequentItemsets)
        //    {
        //        for (int k = 1; k < itemset.Count; k++)
        //        {
        //            var combinations = GetCombinations(itemset, k);

        //            foreach (var combination in combinations)
        //            {
        //                var consequent = itemset.Except(combination).ToList();
        //                var rule = new AssociationRule
        //                {
        //                    Label = $"{string.Join(", ", combination)} => {string.Join(", ", consequent)}"
        //                };

        //                // Tính toán Support
        //                double support = (double)CountItemset(itemset, transactions) / transactions.Count;
        //                if (support >= (double)minSup / transactions.Count)
        //                {
        //                    rule.Support = Math.Round(support * 100, 2);

        //                    // Tính toán Confidence
        //                    int combinationCount = CountItemset(combination, transactions);
        //                    rule.Confidence = combinationCount == 0 ? 0 : Math.Round((double)CountItemset(itemset, transactions) * 100 / combinationCount, 2);

        //                    rules.Add(rule);
        //                }
        //            }
        //        }
        //    }

        //    // Sắp xếp luật kết hợp theo Support và Confidence giảm dần
        //    return rules.OrderByDescending(r => r.Support).ThenByDescending(r => r.Confidence).ToList();
        //}

        public static List<AssociationRule> GetAssociationRules(List<List<string>> frequentItemsets, List<string> transactions, int minSup)
        {
            var rules = new List<AssociationRule>();
            var ruleSet = new HashSet<string>(); // Để theo dõi các luật đã thêm vào danh sách

            foreach (var itemset in frequentItemsets)
            {
                for (int k = 1; k < itemset.Count; k++)
                {
                    var combinations = GetCombinations(itemset, k);

                    foreach (var combination in combinations)
                    {
                        var consequent = itemset.Except(combination).ToList();
                        var label = $"{string.Join(", ", combination)} => {string.Join(", ", consequent)}";

                        // Kiểm tra nếu luật đã tồn tại, bỏ qua luật trùng lặp
                        if (ruleSet.Contains(label))
                            continue;

                        var rule = new AssociationRule
                        {
                            Label = label
                        };

                        // Tính toán Support
                        double support = (double)CountItemset(itemset, transactions) / transactions.Count;
                        if (support >= (double)minSup / transactions.Count)
                        {
                            rule.Support = Math.Round(support * 100, 2);

                            // Tính toán Confidence
                            int combinationCount = CountItemset(combination, transactions);
                            rule.Confidence = combinationCount == 0 ? 0 : Math.Round((double)CountItemset(itemset, transactions) * 100 / combinationCount, 2);

                            rules.Add(rule);
                            ruleSet.Add(label); // Thêm luật vào HashSet để tránh trùng lặp
                        }
                    }
                }
            }

            // Sắp xếp luật kết hợp theo Support và Confidence giảm dần
            return rules.OrderByDescending(r => r.Support).ThenByDescending(r => r.Confidence).ToList();
        }
        #endregion

        #region Hiển thị
        // Hiển thị các tập phổ biến
        public static void PrintFrequentItemsets(List<List<string>> frequentItemsets, List<string> transactions, int minSup)
        {
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
                    int count = CountItemset(itemset, transactions);
                    if (count >= minSup)
                    {
                        Console.WriteLine($"Item: {string.Join(", ", itemset)}, Count: {count}");
                    }
                }
            }
        }

        // Hiển thị các luật kết hợp
        public static void PrintAssociationRule(List<AssociationRule> rules)
        {
            Console.WriteLine("\nQuy tắc kết hợp:");
            foreach (var rule in rules)
            {
                Console.WriteLine($"Rule: {rule.Label}, Support: {rule.Support}%, Confidence: {rule.Confidence}%");
            }
        }
        #endregion
    }
}
