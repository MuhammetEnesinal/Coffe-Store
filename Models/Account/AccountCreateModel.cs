

using System.ComponentModel.DataAnnotations;

public class AccountCreateModel
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [Display(Name = "Ad Soyad")]
     public string AdSoyad { get; set; } =null!;
    

    [Required(ErrorMessage = "E-posta adresi zorunludur.")]
    [Display(Name = "E-posta")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }=null!;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [Display(Name = "Şifre")]
    [DataType(DataType.Password)]
    
    public string Password { get; set; } =null!;

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    [Display(Name = "Şifre (Tekrar)")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Şifreler birbiriyle eşleşmiyor.")]
    public string ConfirmPassword { get; set; } =null!;
}
        


