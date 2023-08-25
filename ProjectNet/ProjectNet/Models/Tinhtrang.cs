using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class Tinhtrang
    {
        [Key]
        public int Id { get; set; }
        public string TINHTRANG { get; set; }
    }
}
