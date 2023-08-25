using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class Phuongthucthanhtoan
    {
        [Key]
        public int Id { get; set; }
        public string TENPT { get; set; }
    }
}
