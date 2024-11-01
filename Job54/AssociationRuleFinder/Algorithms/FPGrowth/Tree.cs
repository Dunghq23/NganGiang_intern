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
    }
}