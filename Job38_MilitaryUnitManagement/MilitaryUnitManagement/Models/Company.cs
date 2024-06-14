namespace MilitaryUnitManagement.Models
{
    public class Company
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int FK_BattalionID { get; set; }
        public string Description { get; set; }
        public Company() { }
        public Company(int id)
        {
            ID = id;
        }

        public Company(int id, string name, string description, int fkBattalionID)
        {
            ID = id;
            Name = name;
            Description = description;
            FK_BattalionID = fkBattalionID;
        }
    }
}
