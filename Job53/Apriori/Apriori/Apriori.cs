﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace AprioriAlgorithm
//{
//    public class Apriori
//    {
//        private List<string> list;
//        private List<string> distinctValues;
//        private List<ItemSet> itemSets;
//        private int minSupportCount;

//        // 1. Constructor mới nhận List<string> làm đầu vào
//        public Apriori(List<string> transactions, int minSup)
//        {
//            list = transactions.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
//            itemSets = new List<ItemSet>();
//            SetDistinctValues(list);
//            this.minSupportCount = minSup;
//        }

//        // 2. Setup Method: Thiết lập danh sách các giá trị duy nhất
//        public void SetDistinctValues(List<string> values)
//        {
//            var data = new HashSet<string>();

//            foreach (var item in values)
//            {
//                var row = item.Split(' ');

//                foreach (var value in row)
//                {
//                    if (!string.IsNullOrWhiteSpace(value))
//                        data.Add(value);
//                }
//            }

//            distinctValues = data.OrderBy(a => a).ToList();
//        }

//        // 3. Core Method: Tìm tập mục phổ biến dựa trên chiều dài (length) và ngưỡng hỗ trợ (support)
//        public ItemSet GetItemSet(int length, int support, bool candidates = false, bool isFirstItemList = false)
//        {
//            var result = GetPermutations(distinctValues, length);
//            //HashSet<IEnumerable<string>> result = temp.ToHashSet();
//            var data = result.Select(p => p.ToList()).ToList();

//            var itemSet = new ItemSet
//            {
//                Support = support,
//                Label = (candidates ? "C" : "L") + length.ToString()
//            };

//            foreach (var item in data)
//            {
//                int count = list.Count(word => item.All(i => word.Split(' ').Contains(i)));

//                if ((candidates && count > 0) || (isFirstItemList && count >= support) || count >= support)
//                {
//                    itemSet[item] = count;
//                    itemSets.Add(itemSet);
//                }

//            }
//            return itemSet;
//        }

//        // 4. Helper Method: Tìm tất cả các tổ hợp (combinations) có độ dài count từ danh sách đầu vào
//        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
//        {
//            int i = 0;

//            foreach (var item in items)
//            {
//                if (count == 1)
//                    yield return new T[] { item };
//                else
//                {
//                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
//                        yield return new T[] { item }.Concat(result);
//                }
//                ++i;
//            }
//        }


//        private static List<List<T>> GetCombinations<T>(List<T> list, int k)
//        {
//            var result = new List<List<T>>();
//            GetCombinations(list, new List<T>(), k, 0, result);
//            return result;
//        }

//        private static void GetCombinations<T>(List<T> list, List<T> tempList, int k, int start, List<List<T>> result)
//        {
//            if (tempList.Count == k)
//            {
//                result.Add(new List<T>(tempList));
//                return;
//            }

//            for (int i = start; i < list.Count; i++)
//            {
//                tempList.Add(list[i]);
//                GetCombinations(list, tempList, k, i + 1, result);
//                tempList.RemoveAt(tempList.Count - 1);
//            }
//        }

//        private int CountItemset(List<string> itemset, List<string> database)
//        {
//            return database.Count(transaction => itemset.All(item => transaction.Split(' ').Select(i => i.Trim()).Contains(item)));
//        }

//        public List<AssociationRule> GetAssociationRules(List<List<string>> frequentItemsets, List<string> database)
//        {
//            var rules = new List<AssociationRule>();

//            foreach (var itemset in frequentItemsets)
//            {
//                for (int k = 1; k < itemset.Count; k++)
//                {
//                    var combinations = GetCombinations(itemset, k);
//                    foreach (var combination in combinations)
//                    {
//                        var consequent = itemset.Except(combination).ToList();
//                        var rule = new AssociationRule
//                        {
//                            Label = string.Join(", ", combination) + " => " + string.Join(", ", consequent)
//                        };

//                        // Tính Support và kiểm tra điều kiện
//                        double support = (double)CountItemset(itemset, database) / database.Count;
//                        if (support >= (double)minSupportCount / database.Count)
//                        {
//                            rule.Support = Math.Round(support * 100, 2);

//                            // Tính Confidence
//                            int combinationCount = CountItemset(combination, database);
//                            rule.Confidence = combinationCount == 0 ? 0 : Math.Round((double)CountItemset(itemset, database) * 100 / combinationCount, 2);

//                            rules.Add(rule);
//                        }
//                    }
//                }
//            }

//            return rules.OrderByDescending(r => r.Support).ThenByDescending(r => r.Confidence).ToList();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AprioriAlgorithm
{
    public class Apriori
    {
        private List<string> list;
        private List<string> distinctValues;
        private List<ItemSet> itemSets;

        // 1. Constructor mới nhận List<string> làm đầu vào
        public Apriori(List<string> transactions)
        {
            list = transactions.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
            itemSets = new List<ItemSet>();
            SetDistinctValues(list);
        }

        // 2. Setup Method: Thiết lập danh sách các giá trị duy nhất
        public void SetDistinctValues(List<string> values)
        {
            var data = new HashSet<string>();

            foreach (var item in values)
            {
                var row = item.Split(' ');

                foreach (var value in row)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                        data.Add(value);
                }
            }

            distinctValues = data.OrderBy(a => a).ToList();
        }

        // 3. Core Method: Tìm tập mục phổ biến dựa trên chiều dài (length) và ngưỡng hỗ trợ (support)
        public ItemSet GetItemSet(int length, int support, bool candidates = false, bool isFirstItemList = false)
        {
            var result = GetPermutations(distinctValues, length).ToList();
            var data = result.Select(p => p.ToList()).ToList();

            var itemSet = new ItemSet
            {
                Support = support,
                Label = (candidates ? "C" : "L") + length.ToString()
            };

            foreach (var item in data)
            {
                int count = list.Count(word => item.All(i => word.Split(' ').Contains(i)));

                if ((candidates && count > 0) || (isFirstItemList && count >= support) || count >= support)
                {
                    itemSet[item] = count;
                    itemSets.Add(itemSet);
                }
            }

            return itemSet;
        }

        // 4. Helper Method: Tìm tất cả các tổ hợp (combinations) có độ dài count từ danh sách đầu vào
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;

            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }

        // 5. Tìm các luật kết hợp từ tập mục phổ biến
        public List<AssociationRule> GetRules(ItemSet items)
        {
            var rules = new List<AssociationRule>();

            foreach (var item in items)
            {
                foreach (var set in item.Key)
                {
                    rules.Add(GetSingleRule(set, item));

                    if (item.Key.Count > 2)
                        rules.Add(GetSingleRule(string.Join(", ", item.Key.Except(new[] { set })), item));
                }
            }

            return rules.OrderByDescending(r => r.Support).ThenByDescending(r => r.Confidence).ToList();
        }

        // 6. Tạo một luật kết hợp đơn lẻ từ tập mục
        private AssociationRule GetSingleRule(string set, KeyValuePair<List<string>, int> item)
        {
            var setItems = set.Split(',').Select(x => x.Trim()).ToList();
            var rule = new AssociationRule();
            var sb = new StringBuilder();

            sb.Append(set).Append(" => ");
            var list = item.Key.Except(setItems).ToList();
            sb.Append(string.Join(", ", list));

            rule.Label = sb.ToString();

            // Tính toán hỗ trợ của luật
            int totalSet = 0;
            var foundSetItem = itemSets.FirstOrDefault(setItem => setItem.ContainsKey(setItems));
            if (foundSetItem != null)
            {
                foundSetItem.TryGetValue(setItems, out totalSet);
            }

            foreach (var first in itemSets)
            {
                var myItem = first.Keys.Where(a => a.ToDisplay() == set);
                if (myItem.Count() > 0)
                {
                    first.TryGetValue(myItem.FirstOrDefault(), out totalSet);
                    break;
                }
            }

            rule.Confidence = Math.Round(((double)item.Value / totalSet) * 100, 2);
            rule.Support = Math.Round(((double)item.Value / this.list.Count) * 100, 2);

            return rule;
        }
    }
}
