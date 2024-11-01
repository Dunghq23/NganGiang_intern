using System;
using System.Collections.Generic;

namespace Algorithms.FPGrowth
{
    public class Tree
    {
        public Node Root { get; private set; } // Gốc của cây
        private readonly int _minSupportCount; // Ngưỡng hỗ trợ tối thiểu

        public Tree(int minSupport)
        {
            Root = new Node(); // Khởi tạo nút gốc
            _minSupportCount = minSupport; // Thiết lập ngưỡng hỗ trợ tối thiểu
        }
        public void BuildTree(ref List<string> elements)
        {
            Node currentNode = Root;

            foreach (var element in elements)
            {
                if (currentNode.Children.ContainsKey(element))
                {
                    // Nếu phần tử đã tồn tại, tăng số lần xuất hiện
                    currentNode.Children[element].Count++;
                    currentNode = currentNode.Children[element];
                }
                else
                {
                    // Nếu phần tử chưa tồn tại, tạo nút mới
                    var newNode = new Node { ItemData = new Item(element), Count = 1 };
                    currentNode.Children[element] = newNode;
                    currentNode = newNode;
                }
            }
        }
        public void PrintTree(Node node, string indent = "")
        {
            if (node == null) return;

            foreach (var child in node.Children)
            {
                Console.WriteLine($"{indent}{child.Key} ({child.Value.Count})");
                PrintTree(child.Value, indent + "  "); // Đệ quy để in các nút con
            }
        }
        public void FindPaths(Node node, List<Item> currentPattern, List<List<Item>> allPaths)
        {
            if (node == null) return;

            foreach (var child in node.Children)
            {
                var newPattern = new List<Item>(currentPattern)
                {
                    new Item(child.Key, child.Value.Count) // Thêm phần tử vào đường đi mới
                };

                allPaths.Add(newPattern); // Lưu đường đi vào danh sách
                FindPaths(child.Value, newPattern, allPaths); // Tiếp tục đệ quy với các con
            }
        }

        //public int CountPattern(Node node, List<Item> pattern)
        //{
        //    int count = 0;

        //    //TraverseAndCount(node, pattern, 0, ref count);
        //    foreach (var child in node.Children.Values)
        //    {
        //        TraverseAndCount(child, pattern, 0, ref count);
        //    }

        //    return count;
        //}

        //private void TraverseAndCount(Node node, List<Item> pattern, int patternIndex, ref int count)
        //{
        //    // Nếu đã tìm hết các phần tử trong pattern, tăng biến đếm
        //    if (patternIndex == pattern.Count)
        //    {
        //        count += node.Count;
        //        return;
        //    }
        //    string currentItemName = pattern[patternIndex].Name;


        //    if (node.ItemData != null && node.ItemData.Name == currentItemName)
        //    {
        //        patternIndex++;
        //    }

        //    if (node.Children != null)
        //    {
        //        foreach (var child in node.Children.Values)
        //        {
        //            TraverseAndCount(child, pattern, patternIndex, ref count);
        //        }
        //    }
        //}

        public int CountPattern(Node node, List<Item> pattern)
        {
            int count = 0;

            // Gọi hàm đệ quy phụ trợ để duyệt cây
            TraverseAndCount(node, pattern, 0, ref count);

            return count;
        }

        // Hàm phụ trợ đệ quy để duyệt và đếm
        private void TraverseAndCount(Node node, List<Item> pattern, int patternIndex, ref int count)
        {
            // Nếu đã tìm hết các phần tử trong pattern, tăng biến đếm
            if (patternIndex == pattern.Count)
            {
                count += node.Count;
                return;
            }

            // Lấy phần tử hiện tại trong pattern
            string currentItemName = pattern[patternIndex].Name;

            // Kiểm tra tất cả các nút con của node hiện tại
            foreach (var child in node.Children.Values)
            {
                // Nếu tên của nút con trùng với tên phần tử hiện tại
                if (child.ItemData != null)
                {
                    //if (child.ItemData.Name == currentItemName)
                    if (checkContains(child.ItemData.Name, pattern))
                    {
                        // Đệ quy tiếp tục với nút con này và phần tử tiếp theo trong pattern
                        TraverseAndCount(child, pattern, patternIndex + 1, ref count);
                    }
                    else
                    {
                        //Tiếp tục tìm phần tử hiện tại của pattern trên nhánh hiện tại
                        TraverseAndCount(child, pattern, patternIndex, ref count);
                    }
                }
            }
        }
        private bool checkContains(string nameNode, List<Item> pattern)
        {
            for (int i = 0; i < pattern.Count; i++)
            {
                if (pattern[i].Name == nameNode)
                    return true;
            }
            return false;
        }
    }
}