using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class Hoadon
    {
        [Key]
        public int Id { get; set; }
        public int IDKH { get; set; }
        public DateTime NGAYDATHANG { get; set; }
        public DateTime NGAYGIAOHANG { get; set; }
        public string DIACHIGIAOHANG { get; set; }
        public int IDNV { get; set; }
        public int IDPT { get; set; }
        public int IDTT { get; set; }
        /*public ICollection<Chitiethoadon>? ChitiethoadonList { get; set; }*/
    }
}
