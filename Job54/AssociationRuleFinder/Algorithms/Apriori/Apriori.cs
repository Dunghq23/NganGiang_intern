//using Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Algorithms.Apriori
//{
//    public class Apriori
//    {
//        private List<string> transactions;
//        private List<string> distinctValues;
//        private int minSupportCount;

//        // 1. Constructor: Khởi tạo với danh sách giao dịch và ngưỡng hỗ trợ tối thiểu
//        public Apriori(List<string> transactions, int minSupport)
//        {
//            this.transactions = transactions.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
//            minSupportCount = minSupport;
//            SetDistinctValues();
//        }

//        // 2. Thiết lập danh sách các giá trị duy nhất từ giao dịch
//        private void SetDistinctValues()
//        {
//            distinctValues = transactions
//                             .SelectMany(t => t.Split(' '))
//                             .Where(v => !string.IsNullOrWhiteSpace(v))
//                             .Distinct()
//                             .OrderBy(a => a)
//                             .ToList();
//        }

//        // 3. Tìm tập mục phổ biến theo độ dài và ngưỡng hỗ trợ
//        public ItemSet GetItemSet(int length, int support)
//        {
//            var itemSet = new ItemSet { Support = support, Label = "L" + length };
//            var combinations = GetPermutations(distinctValues, length).Select(p => p.ToList());

//            foreach (var combination in combinations)
//            {
//                int count = transactions.Count(t => combination.All(item => t.Split(' ').Contains(item)));
//                if (count >= support) itemSet[combination] = count;
//            }

//            return itemSet;
//        }

//        // 4. Helper: Tìm tất cả tổ hợp có độ dài count từ danh sách đầu vào
//        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
//        {
//            return count == 1
//                ? items.Select(item => new T[] { item })
//                : items.SelectMany((item, i) => GetPermutations(items.Skip(i + 1), count - 1).Select(p => new[] { item }.Concat(p)));
//        }

//        // 5. Tìm tất cả các tập mục phổ biến
//        public List<List<string>> GetFrequentItemSet()
//        {
//            int length = 1;
//            var frequentItemSets = new List<List<string>>();

//            while (true)
//            {
//                var itemSet = GetItemSet(length, minSupportCount);
//                if (!itemSet.Any()) break;
//                frequentItemSets.AddRange(itemSet.Keys);
//                length++;
//            }

//            return frequentItemSets;
//        }
//    }
//}




using Common;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Apriori
{
    public class Apriori
    {
        private List<string> transactions;  // Danh sách giao dịch đã được tiền xử lý
        private List<string> distinctValues;  // Các giá trị mục riêng biệt
        private int minSupportCount;  // Ngưỡng hỗ trợ tối thiểu

        // 1. Constructor: Khởi tạo với danh sách giao dịch và ngưỡng hỗ trợ tối thiểu
        public Apriori(List<string> transactions, int minSupport)
        {
            // Tiền xử lý danh sách giao dịch để chuẩn hóa định dạng
            this.transactions = transactions;
            minSupportCount = minSupport;
            SetDistinctValues();
        }

        // 2. Thiết lập danh sách các giá trị duy nhất từ các giao dịch đã tiền xử lý
        private void SetDistinctValues()
        {
            distinctValues = transactions
                             .SelectMany(t => t.Split(','))  // Tách từng mục trong giao dịch (dựa trên dấu ',')
                             .Where(v => !string.IsNullOrWhiteSpace(v))
                             .Distinct()
                             .OrderBy(a => a)
                             .ToList();
        }

        // 3. Tìm tập mục phổ biến theo độ dài và ngưỡng hỗ trợ
        public ItemSet GetItemSet(int length, int support)
        {
            var itemSet = new ItemSet { Support = support, Label = "L" + length };
            var combinations = GetPermutations(distinctValues, length).Select(p => p.ToList());

            foreach (var combination in combinations)
            {
                // Kiểm tra xem các mục của tập hợp có xuất hiện trong tất cả các giao dịch không
                int count = transactions.Count(t => combination.All(item => t.Split(',').Contains(item)));
                if (count >= support)
                    itemSet[combination] = count;
            }

            return itemSet;
        }

        // 4. Helper: Tìm tất cả các tổ hợp có độ dài count từ danh sách đầu vào
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            return count == 1
                ? items.Select(item => new T[] { item })
                : items.SelectMany((item, i) => GetPermutations(items.Skip(i + 1), count - 1).Select(p => new[] { item }.Concat(p)));
        }

        // 5. Tìm tất cả các tập mục phổ biến
        public List<List<string>> GetFrequentItemSet()
        {
            int length = 1;
            var frequentItemSets = new List<List<string>>();

            while (true)
            {
                var itemSet = GetItemSet(length, minSupportCount);
                if (!itemSet.Any()) break;  // Nếu không còn tập mục phổ biến nào, dừng vòng lặp
                frequentItemSets.AddRange(itemSet.Keys);
                length++;
            }

            return frequentItemSets;
        }
    }
}
