
using System.Threading.Tasks;
using CoffeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeStore.Controllers;

[Authorize(Roles = "Admin")]
public class SliderController : Controller
{
    private readonly DataContext _context;

    public SliderController(DataContext context)
    {
        _context = context;
    }
    public ActionResult Index()
    {
        return View(_context.Sliders.Select(i => new SliderGetModel
        {
            Id = i.Id,
            Baslik = i.Baslik,
            Aktif = i.Aktif,
            Aciklama = i.Aciklama,
            ResimUrl = i.ResimUrl
        }).ToList());
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(SliderCreateModel model)
    {
        if (model.Resim == null || model.Resim.Length == 0)
        {
            ModelState.AddModelError("Resim", "Resim seçmelisiniz");
        }

        if (ModelState.IsValid)
        {
            var fileName = Path.GetRandomFileName() + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Resim!.CopyToAsync(stream);
            }
            var slider = new Slider
            {
                Baslik = model.Baslik,
                Aciklama = model.Aciklama,
                ResimUrl = fileName,
                Aktif = model.Aktif
            };
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(model);
    }


    public ActionResult Edit(int id)
    {
        var slider = _context.Sliders.Find(id);
        if (slider == null)
        {
            return NotFound();
        }
        var model = new SliderEditModel
        {
            Id = slider.Id,
            Baslik = slider.Baslik,
            Aciklama = slider.Aciklama,
            ResimUrl = slider.ResimUrl,
            Aktif = slider.Aktif
        };
        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, SliderEditModel model)
    {
        var entity = _context.Sliders.FirstOrDefault(i => i.Id == model.Id);

        if (entity != null)
        {
            if (model.Resim != null)
            {
                var fileName = Path.GetRandomFileName() + ".jpg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Resim!.CopyToAsync(stream);
                }

                entity.ResimUrl = fileName;
            }

            entity.Baslik = model.Baslik;
            entity.Aciklama = model.Aciklama;
            entity.Aktif = model.Aktif;

            _context.SaveChanges();

            TempData["Mesaj"] = "slider güncellendi.";

            return RedirectToAction("Index");
        }
        return View(model);


    }



    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("List");
        }

        var entity = _context.Sliders.FirstOrDefault(i => i.Id == id);

        if (entity != null)
        {
            return View(entity);

        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteConfirm(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("List");
        }

        var entity = _context.Sliders.FirstOrDefault(i => i.Id == id);

        if (entity != null)
        {
            _context.Sliders.Remove(entity);
            _context.SaveChanges();
            TempData["Mesaj"] = "Slider silindi.";
        }
        return RedirectToAction("Index");

    }



}
