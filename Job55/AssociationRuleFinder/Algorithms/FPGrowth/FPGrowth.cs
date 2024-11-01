using Algorithms.FPGrowth;
using System;
using System.Diagnostics;

#region Phiên bản Cleancode
//namespace Algorithms.FPGrowth
//{
//    public class FPGrowth
//    {
//        private readonly int minSupportCount;
//        private List<string> transactions;

//        public FPGrowth(List<string> transactions, int support)
//        {
//            minSupportCount = support;
//            this.transactions = PreprocessDatabase(transactions);
//        }

//        public List<string> PreprocessDatabase(List<string> transactions)
//        {
//            return transactions.Select(t => string.Join(",", t.Split(' ').Select(item => item.Trim()))).ToList();
//        }

//        public List<List<string>> GetFrequentItemsets()
//        {
//            var l1 = GenerateC1(transactions);
//            var fpTree = BuildFPTree(transactions, l1);
//            var pathList = GetPathsFromTree(fpTree);
//            var conditionalPatternBase = GetConditionalPatternBase(pathList, l1);

//            var frequentItemsets = new HashSet<string>();
//            foreach (var itemList in conditionalPatternBase)
//            {
//                foreach (var combination in itemList.SelectMany((_, i) => Helper.GetCombinations(itemList, i + 1)))
//                {
//                    var key = string.Join(", ", combination.Select(i => i.Name));
//                    frequentItemsets.Add(key);
//                }
//            }

//            return frequentItemsets.Select(set => set.Split(',').Select(i => i.Trim()).ToList())
//                                   .OrderBy(set => set.Count)
//                                   .ThenBy(set => string.Join("", set.OrderBy(i => i)))
//                                   .ToList();
//        }

//        private List<Item> GenerateC1(List<string> database)
//        {
//            return database.SelectMany(t => t.Split(',').Select(i => i.Trim()))
//                           .GroupBy(i => i)
//                           .Select(g => new Item(g.Key) { Count = g.Count() })
//                           .Where(item => item.Count >= minSupportCount)
//                           .OrderByDescending(item => item.Count)
//                           .ThenBy(item => item.Name)
//                           .ToList();
//        }

//        private Tree BuildFPTree(List<string> database, List<Item> l1)
//        {
//            var tree = new Tree(minSupportCount);
//            foreach (var transaction in database)
//            {
//                var transactionItems = l1.Where(item => transaction.Contains(item.Name))
//                                         .Select(item => item.Name)
//                                         .ToList();

//                if (transactionItems.Any())
//                {
//                    // Tạo bản sao của transactionItems trước khi truyền vào ref
//                    var itemsCopy = new List<string>(transactionItems);
//                    tree.BuildTree(ref itemsCopy);
//                }
//            }
//            return tree;
//        }


//        private List<List<Item>> GetPathsFromTree(Tree tree)
//        {
//            var pathList = new List<List<Item>>();
//            tree.FindPaths(tree.Root, new List<Item>(), pathList);
//            return pathList;
//        }

//        private List<List<Item>> GetConditionalPatternBase(List<List<Item>> paths, List<Item> l1)
//        {
//            return l1.Select(l1Item => paths.Where(path => path.Last().Name == l1Item.Name)
//                                             .SelectMany(path => path)
//                                             .GroupBy(i => i.Name)
//                                             .Select(g => new Item(g.Key) { Count = g.Sum(i => i.Count) })
//                                             .Where(i => i.Count >= minSupportCount)
//                                             .ToList())
//                      .Where(baseSet => baseSet.Any())
//                      .ToList();
//        }
//    }
//}
#endregion

using Common;
using System.Linq;
using System.Collections.Generic;

namespace Algorithms.FPGrowth
{
    public class FPGrowth
    {
        private readonly int minSupportCount;
        private List<string> transactions;
        public FPGrowth(List<string> transactions, int support)
        {
            minSupportCount = support;
            this.transactions = transactions;

        }
        private List<Item> GenerateC1(List<string> database)
        {
            var itemSets = new Dictionary<string, Item>();

            foreach (var transaction in database)
            {
                foreach (var item in transaction.Split(',').Select(item => item.Trim()))
                {
                    if (!itemSets.TryGetValue(item, out var existingItem))
                    {
                        existingItem = new Item(item);
                        itemSets[item] = existingItem;
                    }
                    existingItem.Count++;
                }
            }

            return itemSets.Values.ToList();
        }
        private List<Item> PruneL1(List<Item> c1)
        {
            return c1.Where(item => item.Count >= minSupportCount).ToList();
        }
        private void SortItemsDescending(List<Item> itemList)
        {
            itemList.Sort((x, y) => y.Count == x.Count ? x.Name.CompareTo(y.Name) : y.Count.CompareTo(x.Count));
        }
        private Tree BuildFPTree(List<string> database, List<Item> l1ItemList)
        {
            var tree = new Tree(minSupportCount);

            foreach (var transaction in database)
            {
                var transactionItems = l1ItemList
                    .Where(item => transaction.Contains(item.Name))
                    .Select(item => item.Name)
                    .ToList();

                if (transactionItems.Count > 0)
                {
                    tree.BuildTree(ref transactionItems);
                }
            }

            return tree;
        }
        private List<List<Item>> GetPathsFromTree(Tree tree)
        {
            var pathListByItem = new List<List<Item>>();
            tree.FindPaths(tree.Root, new List<Item>(), pathListByItem);
            return pathListByItem;
        }
        private List<List<Item>> GetConditionalPatternBase(List<List<Item>> pathList, List<Item> l1ItemList)
        {
            var conditionalPatternBase = new List<List<Item>>();
            foreach (var l1Item in l1ItemList)
            {
                var itemCounts = new Dictionary<string, Item>();
                foreach (var path in pathList)
                {
                    if (l1Item.Name == path.Last().Name)
                    {
                        foreach (var pathItem in path)
                        {
                            if (!itemCounts.TryGetValue(pathItem.Name, out var existingItem))
                            {
                                existingItem = new Item(pathItem.Name, path.Last().Count);
                                itemCounts[pathItem.Name] = existingItem;
                            }
                            else
                            {
                                existingItem.Count += path.Last().Count;
                            }
                        }
                    }
                }
                var filteredPath = itemCounts.Values.Where(i => i.Count >= minSupportCount).ToList();
                if (filteredPath.Count > 0)
                {
                    conditionalPatternBase.Add(filteredPath);
                }
            }

            return conditionalPatternBase;
        }
        public List<List<string>> GetFrequentItemset()
        {
            // Tạo C1 và lọc L1
            var c1ItemList = GenerateC1(transactions);
            var l1ItemList = PruneL1(c1ItemList);
            SortItemsDescending(l1ItemList);

            // Xây dựng cây FP-tree từ dữ liệu đã lọc
            var fpTree = BuildFPTree(transactions, l1ItemList);

            // Lấy danh sách các đường đi từ cây FP
            var pathListByItem = GetPathsFromTree(fpTree);

            // Tạo cơ sở mẫu điều kiện từ các đường đi
            var conditionalPatternBase = GetConditionalPatternBase(pathListByItem, l1ItemList);

            // Khai thác tập mục phổ biến
            var frequentItemsets = new List<List<string>>();
            var uniqueCombinations = new HashSet<string>();
            foreach (var itemList in conditionalPatternBase)
            {
                for (int k = 0; k < itemList.Count; k++)
                {
                    var combinations = Helper.GetCombinations(itemList, k + 1);
                    foreach (var combination in combinations)
                    {
                        var combinationKey = string.Join(",", combination.Select(i => i.Name));

                        if (uniqueCombinations.Add(combinationKey))
                        {
                            frequentItemsets.Add(combination.Select(i => i.Name).ToList());
                        }
                    }
                }
            }

            // Lọc các tập phổ biến không đủ số lần xuất hiện
            frequentItemsets = frequentItemsets
                .Where(itemset => Helper.CountItemset(itemset, transactions) >= minSupportCount)
                .Distinct()
                .OrderBy(itemSet => itemSet.Count)                  // Sắp xếp theo độ dài
                .ThenBy(itemSet => string.Join("", itemSet.OrderBy(i => i)))  // Sắp xếp theo thứ tự chữ cái
                .ToList();

            return frequentItemsets;
        }

        public List<ItemSet> GetFrequentItemsets()
        {
            var c1ItemList = GenerateC1(transactions);
            var l1ItemList = PruneL1(c1ItemList);
            SortItemsDescending(l1ItemList);

            var fpTree = BuildFPTree(transactions, l1ItemList);
            List<List<Node>> allBranches = new List<List<Node>>();

            var pathListByItem = GetPathsFromTree(fpTree);
            List<List<Item>> conditionalPatternBase = GetConditionalPatternBase(pathListByItem, l1ItemList);


            var uniqueCombinations = new HashSet<string>();
            var frequentItemsets = new List<ItemSet>();
            
            foreach (var itemList in conditionalPatternBase)
            {
                for (int k = 0; k < itemList.Count; k++)
                {
                    var combinations = Helper.GetCombinations(itemList, k + 1);
                    foreach (var combination in combinations)
                    {
                        var combinationSort = combination.OrderBy(i => i.Name).ToList();
                        var combinationKey = string.Join(",", combinationSort.Select(i => i.Name));

                        if (uniqueCombinations.Add(combinationKey))
                        {
                            int count = fpTree.CountPattern(fpTree.Root, combination.ToList());
                            frequentItemsets.Add(new ItemSet
                            {
                                Label = combinationKey,
                                Support = count
                            });
                        }
                    }
                }
            }

            var sortedFrequentItemsets = frequentItemsets
                .Where(itemSet => itemSet.Support >= minSupportCount)
                .Distinct()
                .OrderBy(itemSet => itemSet.Count)                  // Sắp xếp theo độ dài
                .ThenBy(itemSet => string.Join("", itemSet.OrderBy(i => i)))  // Sắp xếp theo thứ tự chữ cái
                .ToList();

            return sortedFrequentItemsets;
        }
    }
}