using System.ComponentModel.DataAnnotations;

namespace AuthenticationMvc.Models
{
    public class RegisterViewModel :LoginViewModel
    {
        [MinLength(6, ErrorMessage = "Minimum 6 Karakter")]
        [MaxLength(6, ErrorMessage = "Maksimum 6 Karakter")]
        [Required(ErrorMessage = "Repassword  girilmesi zorunludur")]
        [Compare(nameof(Password)] ///karşılaştır diğer şifre alanı ile name of içindeki stringe dönüşştürür
        public string RePassword { get; set; }
    }
}
