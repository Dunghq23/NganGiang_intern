using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprioriAlgorithm
{
    public class AssociationRule
    {
        public string Label { get; set; }      // Luật theo dạng "A => B"
        public double Support { get; set; }    // Hỗ trợ của luật
        public double Confidence { get; set; } // Độ tin cậy của luật
    }
}
