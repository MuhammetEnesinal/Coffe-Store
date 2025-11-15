

using System.ComponentModel.DataAnnotations;


public class UserCreateModel
{
    [Required]
    [Display(Name = "Ad Soyad")]
    
    public string AdSoyad { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;



}