namespace MilitaryUnitManagement.Models
{
    public class Platoon
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int FK_CompanyID { get; set; }
        public string Description { get; set; }
        public Platoon() { }
        public Platoon(int id)
        {
            ID = id;
        }
        public Platoon(int id, string name, string description, int fkCompanyID)
        {
            ID = id;
            Name = name;
            Description = description;
            FK_CompanyID = fkCompanyID;
        }
    }
}