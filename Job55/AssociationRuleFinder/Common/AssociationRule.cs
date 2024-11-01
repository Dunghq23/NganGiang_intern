namespace Common
{
    public class AssociationRule
    {
        public string Label { get; set; }      // Luật theo dạng "A => B"
        public double Support { get; set; }    // Hỗ trợ của luật
        public double Confidence { get; set; } // Độ tin cậy của luật
    }
}
