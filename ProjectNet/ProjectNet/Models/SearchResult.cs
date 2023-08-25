namespace ProjectNet.Models
{
    public class SearchResult
    {
        public string TENSP { get; set; }
        public int SOLUONGTON { get; set; }
        public string IMG { get; set; }
        public string MOTA { get; set; }
        public int GIABAN { get; set; }
        public string CHATLIEU { get; set; }
        public string MAUSAC { get; set; }
        public string BAOHANH { get; set; }
        public LoaiSanPham? LoaiSanPham { get; set; }
    }
}
