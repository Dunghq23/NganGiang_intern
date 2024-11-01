using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPAlgo
{
    /// <summary>
    /// Class đại diện cho một phần tử trong cây, bao gồm tên phần tử và số lần xuất hiện.
    /// </summary>
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
