using SQLite;

namespace Sitting.Models
{
    public class Worker
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Contact { get; set; }
    }
}
