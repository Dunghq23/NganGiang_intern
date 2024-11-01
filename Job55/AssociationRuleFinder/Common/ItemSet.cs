using System.Collections.Generic;

namespace Common
{
    public class ItemSet : Dictionary<List<string>, int>
    {
        public string Label { get; set; }
        public int Support { get; set; }
    }
}