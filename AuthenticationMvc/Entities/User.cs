using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationMvc.Entities
{
    [Table("Users")] //Tablo Adı
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(30)]
        public String? FullName { get; set; } /*= null;*/ //null alabilirse hata vermez bunu yazınca eskiden böyle ? null için bunu ekle
        [Required]
        [StringLength(30)]
        public String UserName { get; set; }
        [Required]
        [StringLength(100)]
        public String Password { get; set; }
        public bool Active { get; set; } = false;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

    }

}
