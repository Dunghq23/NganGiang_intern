using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryUnitManagement.Models
{
    internal class Battalion
    {
        // Properties corresponding to the columns in the Battalion table
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Battalion()
        {
        }

        public Battalion(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Battalion(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return $"Battalion ID: {ID}, Name: {Name}, Description: {Description}";
        }
    }
}
