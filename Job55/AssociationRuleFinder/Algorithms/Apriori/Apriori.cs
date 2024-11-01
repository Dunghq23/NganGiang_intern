using Common;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithms.Apriori
{
    public class Apriori
    {
        private List<string> transactions;  // Danh sách giao dịch đã được tiền xử lý
        private List<string> distinctValues;  // Các giá trị mục riêng biệt
        private int minSupportCount;  // Ngưỡng hỗ trợ tối thiểu

        // Hàm khởi tạo với danh sách giao dịch và ngưỡng hỗ trợ tối thiểu
        public Apriori(List<string> transactions, int minSupport)
        {
            this.transactions = transactions;
            minSupportCount = minSupport;
            SetDistinctValues();
        }

        // Thiết lập danh sách các giá trị duy nhất từ các giao dịch đã tiền xử lý
        private void SetDistinctValues()
        {
            distinctValues = transactions
                             .SelectMany(t => t.Split(','))
                             .Where(v => !string.IsNullOrWhiteSpace(v))
                             .Distinct()
                             .OrderBy(a => a)
                             .ToList();
        }

        // Hàm chính để lấy tất cả tập mục phổ biến
        public List<ItemSet> GetFrequentItemSets()
        {
            var frequentItemSets = new List<ItemSet>();
            var previousLevelItemSets = new List<ItemSet>();

            // Tạo các tập mục phổ biến ban đầu (1-item)
            foreach (var item in distinctValues)
            {
                int count = transactions.Count(t => t.Split(',').Contains(item));
                if (count >= minSupportCount)
                {
                    var itemSet = new ItemSet { Label = item, Support = count };
                    previousLevelItemSets.Add(itemSet);
                }
            }

            // Thêm các tập mục phổ biến ban đầu vào kết quả cuối
            frequentItemSets.AddRange(previousLevelItemSets);

            int k = 2;
            while (previousLevelItemSets.Any())
            {
                // Tạo các tập mục ứng viên k-item từ tập (k-1)-item phổ biến
                var candidateSets = GenerateCandidates(previousLevelItemSets, k);

                // Tính toán số lần xuất hiện của các tập ứng viên bằng xử lý song song
                var frequentCandidates = new ConcurrentBag<ItemSet>();
                Parallel.ForEach(candidateSets, candidate =>
                {
                    var candidateList = candidate.ToList();
                    int count = transactions.Count(t => candidateList.All(item => t.Split(',').Contains(item)));

                    if (count >= minSupportCount)
                    {
                        var itemSet = new ItemSet { Label = string.Join(",", candidateList), Support = count };
                        frequentCandidates.Add(itemSet);
                    }
                });

                // Chuyển ConcurrentBag sang List và thêm vào kết quả cuối cùng
                previousLevelItemSets = frequentCandidates.ToList();
                frequentItemSets.AddRange(previousLevelItemSets);

                k++;
            }

            return frequentItemSets;
        }

        // Helper để tạo các tập ứng viên từ các tập mục phổ biến trước đó
        private IEnumerable<IEnumerable<string>> GenerateCandidates(List<ItemSet> previousLevelItemSets, int length)
        {
            var items = previousLevelItemSets.Select(i => i.Label.Split(',').ToList()).ToList();
            var candidates = new HashSet<string>();

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = i + 1; j < items.Count; j++)
                {
                    var candidate = items[i].Union(items[j]).Distinct().OrderBy(x => x).ToList();

                    // Chỉ thêm tập ứng viên có độ dài tương ứng
                    if (candidate.Count == length)
                    {
                        candidates.Add(string.Join(",", candidate));
                    }
                }
            }

            return candidates.Select(c => c.Split(','));
        }
    }
}
