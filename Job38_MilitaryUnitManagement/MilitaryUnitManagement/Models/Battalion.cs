namespace MilitaryUnitManagement.Models
{
    public class Battalion
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Battalion()
        {
        }
        public Battalion(int id)
        {
            ID = id;
        }

        public Battalion(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}
