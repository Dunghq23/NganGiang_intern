namespace Algorithms.FPGrowth
{
    public class Item
    {
        public string Name { get; set; } // Tên phần tử
        public int Count { get; set; } // Số lần phần tử xuất hiện
        public Item(string name)
        {
            Name = name;
            Count = 0;
        }
        public Item(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }
    }
}