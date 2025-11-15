


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CoffeStore.Models
{  
     [Authorize(Roles = "Admin")]
    public class KategoriController: Controller
    {

        private readonly DataContext _context;
        public KategoriController(DataContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var kategoriler = _context.Kategoriler.Select(i => new KategoriGetModel
            {
                Id = i.Id,
                KategoriAdi = i.KategoriAdi,
                Url = i.Url,
                UrunSayisi = i.Uruns.Count
            }).ToList();
            return View(kategoriler);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(KategoriCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Kategori
                {
                    KategoriAdi = model.KategoriAdi,
                    Url = model.Url
                };

                _context.Kategoriler.Add(entity);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var kategori = _context.Kategoriler.Find(id);
            if (kategori == null)
            {
                return RedirectToAction("Index");
            }

            var model = new KategoriEditModel
            {
                Id = kategori.Id,
                KategoriAdi = kategori.KategoriAdi,
                Url = kategori.Url
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(KategoriEditModel model)
        {
            if (ModelState.IsValid)
            {
                var kategori = _context.Kategoriler.Find(model.Id);
                if (kategori == null)
                {
                    return RedirectToAction("Index");
                }

                kategori.KategoriAdi = model.KategoriAdi;
                kategori.Url = model.Url;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var entity = _context.Kategoriler.FirstOrDefault(i => i.Id == id);

            if (entity != null)
            {
                return View(entity);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult DeleteConfirm(int id){
            var entity = _context.Kategoriler.FirstOrDefault(i => i.Id == id);

            if (entity != null)
            {
                _context.Kategoriler.Remove(entity);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

