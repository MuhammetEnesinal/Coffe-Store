

using System.ComponentModel.DataAnnotations;

public class AccountLoginModel
{
    

    [Required(ErrorMessage = "E-posta adresi zorunludur.")]
    [Display(Name = "E-posta")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }=null!;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [Display(Name = "Şifre")]
    [DataType(DataType.Password)]

    public string Password { get; set; } = null!;


    public bool BeniHatirla { get; set; } = true;
}
        


