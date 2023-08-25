using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class KHACHHANG
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Chưa nhập mã khách hàng")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Nhập từ 8 kí tự trở lên")]
        [Display(Name = "Mã khách hàng")]
        public string MAKH { get; set; }
        public string HOTEN { get; set; }
        public string DIACHI { get; set; }
        public string SDT { get; set; }
        public string AVARTAR { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }
        [DataType(DataType.Password)]
        public string PASS { get; set; }
        public KHACHHANG()
        {
            AVARTAR = "TKU.jpg";
        }
        public KHACHHANG(int id,string MAKH,string hOTEN, string dIACHI, string sDT, string aVARTAR, string eMAIL, string pASS)
        {
            Id = id;
            this.MAKH = MAKH;
            HOTEN = hOTEN;
            DIACHI = dIACHI;
            SDT = sDT;
            AVARTAR = aVARTAR;
            EMAIL = eMAIL;
            PASS = pASS;
        }
    }
}
