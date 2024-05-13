namespace EduManager.Models
{
    internal class LessonSubject
    {
        public int Id_Les { get; set; }
        public string Les_Unit { get; set; }
        public string Les_Name { get; set; }
        public int FK_Id_Sub { get; set; }
        public int FK_Id_LS { get; set; }
        public int NumHour { get; set; }

        public LessonSubject() { }

        public LessonSubject(string les_Unit, string les_Name, int fK_Id_Sub, int fK_Id_LS, int numHour)
        {
            Les_Unit = les_Unit;
            Les_Name = les_Name;
            FK_Id_Sub = fK_Id_Sub;
            FK_Id_LS = fK_Id_LS;
            NumHour = numHour;
        }
    }
}
