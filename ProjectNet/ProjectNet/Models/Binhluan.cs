using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class Binhluan
    {
        [Key]
        public int ID { get; set; }
        public int IDKH { get; set; }
        public int IDSP { get; set; }
        public string NOIDUNGBL { get; set; }
        public DateTime THOIGIANBL { get; set; }
    }
}
