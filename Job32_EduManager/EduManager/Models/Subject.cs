using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduManager.Models
{
    internal class Subject
    {
        public string Id_Sub {  get; set; }
        public string Name_Sub { get; set; }

        public Subject() { }

        public Subject(string id_Sub)
        {
            Id_Sub = id_Sub;
        }

        public Subject(string id_Sub, string name_Sub)
        {
            Id_Sub = id_Sub;
            Name_Sub = name_Sub;
        }
    }
}
