

using System.ComponentModel.DataAnnotations;


public class UserEditModel
{
    [Required]
    [Display(Name = "Ad Soyad")]

    public string AdSoyad { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; } = null!;


    [DataType(DataType.Password)]
    [Display(Name = "Parola")]
    public string? Password { get; set; } = null!;


    [DataType(DataType.Password)]
    [Display(Name = "Parola Tekrar")]
    [Compare("Password", ErrorMessage = "Parolalar eşleşmiyor.")]
    public string? PasswordConfirm { get; set; } = null!;

    public IList<string>? SelectedRoles { get; set; } = null!;
}