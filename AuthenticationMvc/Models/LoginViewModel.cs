using System.ComponentModel.DataAnnotations;

namespace AuthenticationMvc.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "kullanici adi girilmesi zorunludur")]
        [StringLength(30, ErrorMessage = "En Fazla 30 Karakter desteklenmektedir")]
        public String UserName { get; set; }
        /// <summary>
        /// /[DataType(DataType.Password)]
        /// </summary>
        [MinLength(6,ErrorMessage ="Minimum 6 Karakter")]
        [MaxLength(6, ErrorMessage = "Maksimum 6 Karakter")]
        [Required(ErrorMessage = "password  girilmesi zorunludur")]
        public String Password { get; set; }
    }
}
