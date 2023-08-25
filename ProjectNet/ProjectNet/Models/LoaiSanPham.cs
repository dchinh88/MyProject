using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class LoaiSanPham
    {
        [Key]
        public int ID { get; set; }
        public string TENLSP { get; set; }
        public ICollection<SanPham>? SanPhamList { get; set; }
    }
}
