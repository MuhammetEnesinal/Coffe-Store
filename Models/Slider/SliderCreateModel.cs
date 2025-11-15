using System.ComponentModel.DataAnnotations;

public class SliderCreateModel
{

    [Required(ErrorMessage = "{0} alanı gereklidir.")]
    [Display(Name = "Başlık")]
    public string Baslik { get; set; } = null!;
    [Required(ErrorMessage = "{0} alanı gereklidir.")]
    [Display(Name = "Açıklama")]
    public string Aciklama { get; set; } = null!;
    [Required(ErrorMessage = "{0} alanı gereklidir.")]
    [Display(Name = "Resim")]
    public IFormFile? Resim { get; set; } = null!;

    [Required(ErrorMessage = "{0} alanı gereklidir.")]
    [Display(Name = "Aktif")]
    public bool Aktif { get; set; } = false;
}