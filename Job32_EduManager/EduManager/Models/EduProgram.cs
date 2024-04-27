using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduManager.Models
{
    internal class EduProgram
    {
        public int Id_EP { get; set; }
        public string FK_Id_Sub { get; set; }
        public int FK_Id_LS { get; set; }
        public int NumHour { get; set; }

        public EduProgram() { }

        public EduProgram(string fK_Id_Sub)
        {
            FK_Id_Sub = fK_Id_Sub;
        }
        public EduProgram(string fK_Id_Sub, int fK_Id_LS, int numHour)
        {
            FK_Id_Sub = fK_Id_Sub;
            FK_Id_LS = fK_Id_LS;
            NumHour = numHour;
        }
        public EduProgram(int id_EP, string fK_Id_Sub, int fK_Id_LS, int numHour)
        {
            Id_EP = id_EP;
            FK_Id_Sub = fK_Id_Sub;
            FK_Id_LS = fK_Id_LS;
            NumHour = numHour;
        }
    }
}
