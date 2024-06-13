
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryUnitManagement.Models
{
    internal class Company
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int FK_BattalionID { get; set; }
        public string Description { get; set; }

        // Constructor for easy instantiation
        public Company(int id, string name, int fkBattalionID, string description)
        {
            ID = id;
            Name = name;
            FK_BattalionID = fkBattalionID;
            Description = description;
        }

        // Parameterless constructor for flexibility
        public Company()
        {
        }

        // Override ToString method for better readability
        public override string ToString()
        {
            return $"Company ID: {ID}, Name: {Name}, Battalion ID: {FK_BattalionID}, Description: {Description}";
        }
    }
}
