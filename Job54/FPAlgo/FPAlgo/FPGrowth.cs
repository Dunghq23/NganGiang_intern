//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;

//namespace FPAlgo
//{
//    class FPGrowth
//    {
//        private List<ItemSet> itemSets;
//        private int MinSupportCount { get; }
//        public double ExecutionTime { get; private set; }

//        public FPGrowth(int support)
//        {
//            MinSupportCount = support;
//        }

//        // Tiền xử lý cơ sở dữ liệu: Làm sạch và cắt dữ liệu
//        public List<string> PreprocessDatabase(string[] database)
//        {
//            return database.Select(trans => string.Join(",", trans.Split(',').Select(item => item.Trim()))).ToList();
//        }

//        // Tìm và in các tập hợp thường xuyên
//        public void RunFPGrowth(List<string> database)
//        {
//            var stopwatch = Stopwatch.StartNew();

//            var c1ItemList = GenerateC1(database);
//            var l1ItemList = PruneL1(c1ItemList);
//            SortItemsDescending(l1ItemList);

//            var fpTree = BuildFPTree(database, l1ItemList);
//            var pathListByItem = GetPathsFromTree(fpTree);

//            var conditionalPatternBase = GetConditionalPatternBase(pathListByItem, l1ItemList);
//            var frequentItemsets = GetFrequentItemsets(conditionalPatternBase);

//            PrintFrequentItemsets(frequentItemsets, database);

//            stopwatch.Stop();
//            ExecutionTime = stopwatch.Elapsed.TotalSeconds;
//        }

//        // Tạo danh sách C1 (tất cả các mục độc nhất cùng với số lần xuất hiện của chúng)
//        private List<Item> GenerateC1(List<string> database)
//        {
//            var itemSets = new Dictionary<string, Item>();

//            foreach (var transaction in database)
//            {
//                foreach (var item in transaction.Split(',').Select(item => item.Trim()))
//                {
//                    if (itemSets.ContainsKey(item))
//                    {
//                        itemSets[item].Count++;
//                    }
//                    else
//                    {
//                        itemSets[item] = new Item(item);
//                    }
//                }
//            }

//            return itemSets.Values.ToList();
//        }

//        // Lọc các mục trong C1 để tạo L1 (các mục thường xuyên)
//        private List<Item> PruneL1(List<Item> c1)
//        {
//            return c1.Where(item => item.Count >= MinSupportCount).ToList();
//        }

//        // Sắp xếp các mục theo tần suất giảm dần
//        private void SortItemsDescending(List<Item> itemList)
//        {
//            itemList.Sort((x, y) => y.Count == x.Count ? x.Name.CompareTo(y.Name) : y.Count.CompareTo(x.Count));
//        }

//        // Xây dựng cây FP từ cơ sở dữ liệu
//        private Tree BuildFPTree(List<string> database, List<Item> l1ItemList)
//        {
//            var tree = new Tree(MinSupportCount);

//            foreach (var transaction in database)
//            {
//                var transactionItems = l1ItemList
//                    .Where(item => transaction.Contains(item.Name))
//                    .Select(item => item.Name)
//                    .ToList();

//                tree.BuildTree(ref transactionItems);
//            }

//            return tree;
//        }

//        // Lấy tất cả các đường đi từ cây FP
//        private List<List<Item>> GetPathsFromTree(Tree tree)
//        {
//            var pathListByItem = new List<List<Item>>();
//            tree.FindPaths(tree.Root, new List<Item>(), pathListByItem);
//            return pathListByItem;
//        }

//        // Lấy cơ sở mẫu điều kiện cho mỗi mục thường xuyên
//        private List<List<Item>> GetConditionalPatternBase(List<List<Item>> pathList, List<Item> l1ItemList)
//        {
//            var conditionalPatternBase = new List<List<Item>>();

//            foreach (var l1Item in l1ItemList)
//            {
//                var itemCounts = new Dictionary<string, Item>();

//                foreach (var path in pathList)
//                {
//                    if (l1Item.Name == path.Last().Name)
//                    {
//                        foreach (var pathItem in path)
//                        {
//                            if (itemCounts.ContainsKey(pathItem.Name))
//                            {
//                                itemCounts[pathItem.Name].Count += path.Last().Count;
//                            }
//                            else
//                            {
//                                itemCounts[pathItem.Name] = new Item(pathItem.Name, path.Last().Count);
//                            }
//                        }
//                    }
//                }

//                var filteredPath = itemCounts.Values.Where(i => i.Count >= MinSupportCount).ToList();
//                if (filteredPath.Count > 0)
//                {
//                    conditionalPatternBase.Add(filteredPath);
//                }
//            }

//            return conditionalPatternBase;
//        }

//        // Tìm các tập hợp thường xuyên từ cơ sở mẫu điều kiện
//        private List<List<string>> GetFrequentItemsets(List<List<Item>> conditionalPatternBase)
//        {
//            var frequentItemsets = new List<List<string>>();
//            var uniqueCombinations = new HashSet<string>();

//            foreach (var itemList in conditionalPatternBase)
//            {
//                for (int k = 0; k < itemList.Count; k++)
//                {
//                    var combinations = GetCombinations(itemList, k + 1);
//                    foreach (var combination in combinations)
//                    {
//                        var combinationKey = string.Join(", ", combination.Select(i => i.Name));
//                        if (uniqueCombinations.Add(combinationKey))
//                        {
//                            frequentItemsets.Add(combination.Select(i => i.Name).ToList());
//                        }
//                    }
//                }
//            }

//            return frequentItemsets;
//        }

//        // Tạo ra các tổ hợp mục
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

//        // In ra các tập hợp thường xuyên
//        private void PrintFrequentItemsets(List<List<string>> frequentItemsets, List<string> database)
//        {
//            var groupedByLength = frequentItemsets
//                .GroupBy(itemset => itemset.Count)
//                .OrderBy(g => g.Key)
//                .ToList();

//            foreach (var group in groupedByLength)
//            {
//                Console.WriteLine($"\nFrequent ItemSet (Length = {group.Key}, Support >= {MinSupportCount}):");
//                foreach (var itemset in group)
//                {
//                    int count = CountItemset(itemset, database);
//                    Console.WriteLine($"Item: {string.Join(", ", itemset)}, Count: {count}");
//                }
//            }
//        }

//        // Đếm số lần xuất hiện của một tập hợp trong các giao dịch
//        private int CountItemset(List<string> itemset, List<string> database)
//        {
//            return database.Count(transaction => itemset.All(item => transaction.Split(',').Select(i => i.Trim()).Contains(item)));
//        }
//    }
//}


using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System;
using System.Security.Cryptography;

//namespace FPAlgo
//{
//    class FPGrowth
//    {
//        private List<ItemSet> itemSets;
//        private int MinSupportCount { get; }
//        public double ExecutionTime { get; private set; }

//        public FPGrowth(int support)
//        {
//            MinSupportCount = support;
//        }

//        // Tiền xử lý cơ sở dữ liệu: Làm sạch và cắt dữ liệu
//        public List<string> PreprocessDatabase(string[] database)
//        {
//            return database.Select(trans => string.Join(",", trans.Split(',').Select(item => item.Trim()))).ToList();
//        }

//        // Tìm và in các tập hợp thường xuyên
//        public void RunFPGrowth(List<string> database)
//        {
//            var stopwatch = Stopwatch.StartNew();

//            var c1ItemList = GenerateC1(database);
//            var l1ItemList = PruneL1(c1ItemList);
//            SortItemsDescending(l1ItemList);

//            var fpTree = BuildFPTree(database, l1ItemList);
//            var pathListByItem = GetPathsFromTree(fpTree);

//            var conditionalPatternBase = GetConditionalPatternBase(pathListByItem, l1ItemList);
//            var frequentItemsets = GetFrequentItemsets(conditionalPatternBase);

//            PrintFrequentItemsets(frequentItemsets, database);

//            stopwatch.Stop();
//            ExecutionTime = stopwatch.Elapsed.TotalSeconds;
//        }

//        // Tạo danh sách C1 (tất cả các mục độc nhất cùng với số lần xuất hiện của chúng)
//        private List<Item> GenerateC1(List<string> database)
//        {
//            var itemSets = new Dictionary<string, Item>();

//            foreach (var transaction in database)
//            {
//                foreach (var item in transaction.Split(',').Select(item => item.Trim()))
//                {
//                    if (itemSets.ContainsKey(item))
//                    {
//                        itemSets[item].Count++;
//                    }
//                    else
//                    {
//                        itemSets[item] = new Item(item);
//                    }
//                }
//            }

//            return itemSets.Values.ToList();
//        }

//        // Lọc các mục trong C1 để tạo L1 (các mục thường xuyên)
//        private List<Item> PruneL1(List<Item> c1)
//        {
//            return c1.Where(item => item.Count >= MinSupportCount).ToList();
//        }

//        // Sắp xếp các mục theo tần suất giảm dần
//        private void SortItemsDescending(List<Item> itemList)
//        {
//            itemList.Sort((x, y) => y.Count == x.Count ? x.Name.CompareTo(y.Name) : y.Count.CompareTo(x.Count));
//        }

//        // Xây dựng cây FP từ cơ sở dữ liệu
//        private Tree BuildFPTree(List<string> database, List<Item> l1ItemList)
//        {
//            var tree = new Tree(MinSupportCount);

//            foreach (var transaction in database)
//            {
//                var transactionItems = l1ItemList
//                    .Where(item => transaction.Contains(item.Name))
//                    .Select(item => item.Name)
//                    .ToList();

//                tree.BuildTree(ref transactionItems);
//            }

//            return tree;
//        }

//        // Lấy tất cả các đường đi từ cây FP
//        private List<List<Item>> GetPathsFromTree(Tree tree)
//        {
//            var pathListByItem = new List<List<Item>>();
//            tree.FindPaths(tree.Root, new List<Item>(), pathListByItem);
//            return pathListByItem;
//        }

//        // Lấy cơ sở mẫu điều kiện cho mỗi mục thường xuyên
//        private List<List<Item>> GetConditionalPatternBase(List<List<Item>> pathList, List<Item> l1ItemList)
//        {
//            var conditionalPatternBase = new List<List<Item>>();

//            foreach (var l1Item in l1ItemList)
//            {
//                var itemCounts = new Dictionary<string, Item>();

//                foreach (var path in pathList)
//                {
//                    if (l1Item.Name == path.Last().Name)
//                    {
//                        foreach (var pathItem in path)
//                        {
//                            if (itemCounts.ContainsKey(pathItem.Name))
//                            {
//                                itemCounts[pathItem.Name].Count += path.Last().Count;
//                            }
//                            else
//                            {
//                                itemCounts[pathItem.Name] = new Item(pathItem.Name, path.Last().Count);
//                            }
//                        }
//                    }
//                }

//                var filteredPath = itemCounts.Values.Where(i => i.Count >= MinSupportCount).ToList();
//                if (filteredPath.Count > 0)
//                {
//                    conditionalPatternBase.Add(filteredPath);
//                }
//            }

//            return conditionalPatternBase;
//        }

//        // Tìm các tập hợp thường xuyên từ cơ sở mẫu điều kiện
//        private List<List<string>> GetFrequentItemsets(List<List<Item>> conditionalPatternBase)
//        {
//            var frequentItemsets = new List<List<string>>();
//            var uniqueCombinations = new HashSet<string>();

//            foreach (var itemList in conditionalPatternBase)
//            {
//                for (int k = 0; k < itemList.Count; k++)
//                {
//                    var combinations = GetCombinations(itemList, k + 1);
//                    foreach (var combination in combinations)
//                    {
//                        var combinationKey = string.Join(", ", combination.Select(i => i.Name));
//                        if (uniqueCombinations.Add(combinationKey))
//                        {
//                            frequentItemsets.Add(combination.Select(i => i.Name).ToList());
//                        }
//                    }
//                }
//            }

//            return frequentItemsets;
//        }

//        // Tạo ra các tổ hợp mục
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

//        // In ra các tập hợp thường xuyên
//        private void PrintFrequentItemsets(List<List<string>> frequentItemsets, List<string> database)
//        {
//            var groupedByLength = frequentItemsets
//                .GroupBy(itemset => itemset.Count)
//                .OrderBy(g => g.Key)
//                .ToList();

//            foreach (var group in groupedByLength)
//            {
//                Console.WriteLine($"\nFrequent ItemSet (Length = {group.Key}, Support >= {MinSupportCount}):");
//                foreach (var itemset in group)
//                {
//                    int count = CountItemset(itemset, database);
//                    Console.WriteLine($"Item: {string.Join(", ", itemset)}, Count: {count}");
//                }
//            }
//        }

//        // Đếm số lần xuất hiện của một tập hợp trong các giao dịch
//        private int CountItemset(List<string> itemset, List<string> database)
//        {
//            return database.Count(transaction => itemset.All(item => transaction.Split(',').Select(i => i.Trim()).Contains(item)));
//        }
//    }
//}

#region CHỐT
namespace FPAlgo
{
    class FPGrowth
    {
        private readonly int minSupportCount;
        private List<string> transaction;
        public double ExecutionTime { get; private set; }

        public FPGrowth(List<string> transactions, int support)
        {
            minSupportCount = support;
            this.transaction = PreprocessDatabase(transactions);
        }

        public List<string> PreprocessDatabase(List<string> database)
        {
            return database.Select(transaction => string.Join(",", transaction.Split(' ').Select(item => item.Trim()))).ToList();
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

        public List<List<string>> GetFrequentItemsets()
        {
            var c1ItemList = GenerateC1(transaction);
            var l1ItemList = PruneL1(c1ItemList);
            SortItemsDescending(l1ItemList);

            var fpTree = BuildFPTree(transaction, l1ItemList);
            var pathListByItem = GetPathsFromTree(fpTree);
            var conditionalPatternBase = GetConditionalPatternBase(pathListByItem, l1ItemList);


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
            frequentItemsets = frequentItemsets
                 .Where(itemset => Helper.CountItemset(itemset, transaction) >= minSupportCount)
                 .Distinct()
                 .OrderBy(itemSet => itemSet.Count)                  // Sắp xếp theo độ dài
                 .ThenBy(itemSet => string.Join("", itemSet.OrderBy(i => i)))  // Sắp xếp theo thứ tự chữ cái
                 .ToList();


            return frequentItemsets;
        }
    }
}
#endregion

