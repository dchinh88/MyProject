using System.ComponentModel.DataAnnotations;

namespace ProjectNet.Models
{
    public class AccountKH
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }
        [DataType(DataType.Password)]
        public string PASS { get; set; }
    }
}
