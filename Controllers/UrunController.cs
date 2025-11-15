using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



namespace CoffeStore.Controllers
{
   [Authorize(Roles = "Admin")]
    public class UrunController : Controller
    {
        private readonly DataContext _context;

        public UrunController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public ActionResult Index(string? url, string? q)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var kategori = _context.Kategoriler
                                    .FirstOrDefault(k => k.Url == url);

                if (kategori == null)
                {
                    return RedirectToAction("Index");
                }

                // Ürünleri kategoriye göre çek
                var urunler = _context.Urunler
                                .Where(u => u.Aktif && u.KategoriId == kategori.Id)
                                .ToList();

                // Eğer q parametresi varsa filtre uygula
                if (!string.IsNullOrEmpty(q))
                {
                    urunler = urunler
                        .Where(u => u.UrunAdi.Contains(q) || u.Aciklama.Contains(q))
                        .ToList();
                }

                ViewData["Kategoriler"] = _context.Kategoriler.ToList();
                ViewData["SeciliKategori"] = kategori;
                ViewData["AramaKelimesi"] = q;

                return View(urunler);
            }
            else
            {
                var urunler = _context.Urunler.ToList();

                if (!string.IsNullOrEmpty(q))
                {
                    urunler = urunler
                        .Where(u => u.UrunAdi.Contains(q) || u.Aciklama.Contains(q))
                        .ToList();
                }

                ViewData["Kategoriler"] = _context.Kategoriler.ToList();
                ViewData["AramaKelimesi"] = q;

                return View(urunler);
            }
        }



        public ActionResult List(int? kategori)
        {

            var query = _context.Urunler.AsQueryable();


            if (kategori != null)
            {
                query = query.Where(u => u.KategoriId == kategori);
            }


            var urunler = query.ToList();


            ViewBag.Kategoriler = new SelectList(
                _context.Kategoriler.ToList(),
                "Id",
                "KategoriAdi",
                kategori
            );


            return View(urunler);
        }



        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var urun = _context.Urunler
            .Include(u => u.Kategori)
            .FirstOrDefault(u => u.Id == id);

            if (urun == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["BenzerUrunler"] = _context.Urunler
                                    .Where(i => i.Aktif && i.KategoriId == urun.KategoriId && i.Id != id)
                                    .Take(4)
                                    .ToList();

            return View(urun);


        }





        public ActionResult Create()
        {
            ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UrunCreateModel model)
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

                var entity = new Urun()
                {
                    UrunAdi = model.UrunAdi,
                    Aciklama = model.Aciklama ?? string.Empty,
                    Fiyat = model.Fiyat ?? 0,
                    Aktif = model.Aktif,
                    Anasayfa = model.Anasayfa,
                    KategoriId = (int)model.KategoriId!,
                    ResimUrl = fileName
                };

                _context.Urunler.Add(entity);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");
            return View(model);
        }



        public ActionResult Edit(int id)
        {
            var entity = _context.Urunler.Select(i => new UrunEditModel
            {
                Id = i.Id,
                UrunAdi = i.UrunAdi,
                Aciklama = i.Aciklama,
                Aktif = i.Aktif,
                Anasayfa = i.Anasayfa,
                Fiyat = i.Fiyat,
                KategoriId = i.KategoriId,
                ResimUrl = i.ResimUrl
            }).FirstOrDefault(i => i.Id == id);

            ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");
            return View(entity);
        }



        [HttpPost]
        public async Task<ActionResult> Edit(int id, UrunEditModel model)
        {
            if (id != model.Id)
            {
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {
                var entity = _context.Urunler.FirstOrDefault(i => i.Id == model.Id);

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

                    entity.UrunAdi = model.UrunAdi;
                    entity.Aciklama = model.Aciklama ?? "";
                    entity.Fiyat = model.Fiyat ?? 0;
                    entity.Aktif = model.Aktif;
                    entity.Anasayfa = model.Anasayfa;
                    entity.KategoriId = (int)model.KategoriId!;

                    _context.SaveChanges();

                    TempData["Mesaj"] = $"{entity.UrunAdi} ürünü güncellendi.";

                    return RedirectToAction("List");
                }

            }

            ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");
            return View(model);
        }



        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("List");
            }

            var entity = _context.Urunler.FirstOrDefault(i => i.Id == id);

            if (entity != null)
            {
                return View(entity);

            }

            return RedirectToAction("List");
        }


        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            var entity = _context.Urunler.FirstOrDefault(i => i.Id == id);

            if (entity != null)
            {
                _context.Urunler.Remove(entity);
                _context.SaveChanges();

                TempData["Mesaj"] = $"{entity.UrunAdi} ürünü silindi.";
            }

            return RedirectToAction("List");
        }




    }
}