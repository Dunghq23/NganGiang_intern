namespace EduManager.Models
{
    internal class Subject
    {
        public int Id_Sub {  get; set; }
        public string Name_Sub { get; set; }
        public string Sym_Sub { get; set; }



        public Subject() { }

        public Subject(int id_Sub)
        {
            Id_Sub = id_Sub;
        }

        public Subject(int id_Sub, string name_Sub, string sym_Sub)
        {
            Id_Sub = id_Sub;
            Name_Sub = name_Sub;
            Sym_Sub = sym_Sub;
        }
    }
}
