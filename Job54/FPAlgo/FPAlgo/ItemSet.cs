using System.Collections.Generic;

namespace FPAlgo
{
    public class ItemSet : Dictionary<List<string>, int>
    {
        public string Label { get; set; }
        public int Support { get; set; }
    }
}
