

using System.ComponentModel.DataAnnotations;

public class AccountChangePasswordModel
{
    [Required(ErrorMessage = "Şifre zorunludur.")]
    [Display(Name = "Şifre")]
    [DataType(DataType.Password)]  
    public string OldPassword { get; set; } =null!;



    [Required(ErrorMessage = "Şifre zorunludur.")]
    [Display(Name = "Şifre")]
    [DataType(DataType.Password)]  public string Password { get; set; } =null!;

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    [Display(Name = "Şifre (Tekrar)")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Şifreler birbiriyle eşleşmiyor.")]
    public string ConfirmPassword { get; set; } =null!;
}
