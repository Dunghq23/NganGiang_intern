using System.Collections.Generic;

namespace Algorithms.FPGrowth
{
    public class Node
    {
        public Dictionary<string, Node> Children { get; set; } // Danh sách các nút con
        public Item ItemData { get; set; } // Dữ liệu của phần tử tại nút này
        public int Count { get; set; } // Số lần xuất hiện của phần tử
        public Node Parent { get; set; } // Thêm thuộc tính để chỉ định nút cha
        public List<Node> NodeLink { get; set; } // Danh sách liên kết nút trỏ về các phần tử tương ứng
        public Node()
        {
            Children = new Dictionary<string, Node>();
            ItemData = null;
            Count = 0;
            Parent = null;
            NodeLink = new List<Node>();
        }
    }
}