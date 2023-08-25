
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ProjectNet.Models
{
    public class SanPham
    {
        [Key]
        public int ID { get; set; }
        /*[Required(ErrorMessage = "Chưa nhập số lượng tồn")]*/
        [Display(Name = "Loại sản phẩm")]
        public int LOAISANPHAMID { get; set; }
        [Required(ErrorMessage = "Chưa nhập mã sản phẩm")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Nhập từ 8 kí tự trở lên")]
        [Display(Name = "Mã sản phẩm")]
        public string MASP { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string TENSP { get; set; }
        [Required(ErrorMessage = "Chưa nhập số lượng tồn")]
        [Display(Name = "Số lượng tồn")]
        public int SOLUONGTON { get; set; }
        public string IMG { get; set; }
        [Required(ErrorMessage = "Chưa nhập mô tả")]
        [Display(Name = "Mô tả")]
        public string MOTA { get; set; }
        [Required(ErrorMessage = "Chưa nhập giá bán")]
        [Display(Name = "Giá bán")]
        public int GIABAN { get; set; }
        [Required(ErrorMessage = "Chưa nhập chất liệu")]
        [Display(Name = "Chất liệu")]
        public string CHATLIEU { get; set; }
        [Required(ErrorMessage = "Chưa nhập màu sắc")]
        [Display(Name = "Màu sắc")]
        public string MAUSAC { get; set; }
        [Required(ErrorMessage = "Chưa nhập bảo hành")]
        [Display(Name = "Bảo hành")]
        public string BAOHANH { get; set; }
        public LoaiSanPham? LoaiSanPham { get; set; }
        

        public SanPham()
        {
            IMG = "loi.png";
        }
        public SanPham(int iD, int lOAISANPHAMID, string mASP, string tENSP, int sOLUONGTON, string iMG, string mOTA, int gIABAN, string cHATLIEU, string mAUSAC, string bAOHANH, LoaiSanPham? loaiSanPham)
        {
            ID = iD;
            LOAISANPHAMID = lOAISANPHAMID;
            MASP = mASP;
            TENSP = tENSP;
            SOLUONGTON = sOLUONGTON;
            IMG = iMG;
            MOTA = mOTA;
            GIABAN = gIABAN;
            CHATLIEU = cHATLIEU;
            MAUSAC = mAUSAC;
            BAOHANH = bAOHANH;
            LoaiSanPham = loaiSanPham;
        }
        
    }
}
