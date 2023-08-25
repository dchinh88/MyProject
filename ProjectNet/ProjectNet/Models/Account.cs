using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ban phai nhap username")]

        public string TENDN { get; set; }
        [Required(ErrorMessage = "Ban phai nhap password")]

        public string PASS { get; set; }

    }
}
