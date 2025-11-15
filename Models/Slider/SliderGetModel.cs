namespace CoffeStore.Models;
public class SliderGetModel
{
    public int Id { get; set; }
    public string ResimUrl { get; set; } = null!;
    public bool Aktif { get; set; }
    public string Baslik { get; set; } = null!;
    public string Aciklama { get; set; } = null!;
}