using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class NhanVien
    {
        [Key]
        public int ID { get; set; }
        public string MANV { get; set; }
        public string HOTEN { get; set; }
        public DateTime NGAYSINH { get; set; }
        public string DIACHI { get; set; }
        public string SDT { get; set; }
        public string AVARTAR { get; set; }
        public string EMAIL { get; set; }
        [Required(ErrorMessage = "Ban phai nhap username")]
        public string TENDN { get; set; }
        [Required(ErrorMessage = "Ban phai nhap password")]
        public string PASS { get; set; }
        public bool ISADMIN { get; set; }
        public NhanVien()
        {
            AVARTAR = "default.jpg";
        }
       /* public NhanVien(int iD, string mANV, string hOTEN, DateTime nGAYSINH, string dIACHI, string sDT, string aVARTAR, string eMAIL, string tENDN, string pASS, bool iSADMIN)
        {
            ID = iD;
            MANV = mANV;
            HOTEN = hOTEN;
            NGAYSINH = nGAYSINH;
            DIACHI = dIACHI;
            SDT = sDT;
            AVARTAR = aVARTAR;
            EMAIL = eMAIL;
            TENDN = tENDN;
            PASS = pASS;
            ISADMIN = iSADMIN;
        }*/
    }
}
