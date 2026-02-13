using System.ComponentModel.DataAnnotations.Schema;

namespace preTest05.Model
{
    [Table("queues")]
    public class Queues
    {
        public int id { get; set; }
        public char prefix { get; set; }
        public int number { get; set; }
        [Column("createdat")]
        public DateTime createdAt { get; set; }
    }
}
