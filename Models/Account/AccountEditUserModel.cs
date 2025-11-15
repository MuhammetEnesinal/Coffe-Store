

using System.ComponentModel.DataAnnotations;

public class AccountEditUserModel
{
    

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [Display(Name = "Ad Soyad")]
     public string AdSoyad { get; set; } =null!;

    [Required(ErrorMessage = "E-posta adresi zorunludur.")]
    [Display(Name = "E-posta")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }=null!;

}