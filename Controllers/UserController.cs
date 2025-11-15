using System.Threading.Tasks;
using CoffeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoffeStore.Controllers;

    
[Authorize(Roles = "Admin")]
public class UserController : Controller
{

    private readonly UserManager<AppUser> _usermanager;
    private readonly RoleManager<AppRole> _roleManager;



    public UserController(UserManager<AppUser> usermanager, RoleManager<AppRole> roleManager)
    {
        _usermanager = usermanager;
        _roleManager = roleManager;

    }


    public async Task<ActionResult> Index(string role)
    {
        ViewBag.Roles= new SelectList(_roleManager.Roles, "Name", "Name",role);
        if(!string.IsNullOrEmpty(role))
        {
            var usersInRole =await  _usermanager.GetUsersInRoleAsync(role);
            return View(usersInRole);
        }

        return View(_usermanager.Users);
    }


    public ActionResult Create()
    {

        return View();
    }


    [HttpPost]

    public async Task<ActionResult> Create(UserCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                AdSoyad = model.AdSoyad

            };

            var result = await _usermanager.CreateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View();
    }


    public async Task<ActionResult> Edit(string id)
    {
        var user = _usermanager.FindByIdAsync(id).Result;
        if (user == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

        return View(new UserEditModel
        {
            AdSoyad = user.AdSoyad,
            Email = user.Email,
            SelectedRoles =await _usermanager.GetRolesAsync(user),
        });


    }

    [HttpPost]
    public async Task<ActionResult> Edit(UserEditModel model, string id)
    {
        if (ModelState.IsValid)
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user != null)
            {
                user.AdSoyad = model.AdSoyad;
                user.Email = model.Email;
                var result = await _usermanager.UpdateAsync(user);

                if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                {
                    await _usermanager.RemovePasswordAsync(user);
                    await _usermanager.AddPasswordAsync(user, model.Password);
                }

                if (result.Succeeded)
                {

                    var userRoles = await _usermanager.RemoveFromRolesAsync(user, await _usermanager.GetRolesAsync(user));

                    if (model.SelectedRoles != null && model.SelectedRoles.Count > 0)
                    {
                        var addRoles = await _usermanager.AddToRolesAsync(user, model.SelectedRoles);
                    }
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }


        }
        return View(model);
    }


    public async Task<ActionResult> Delete(string id)
    {
        if (id == null)
        {

            return RedirectToAction("Index");
        }
        var entity = await _usermanager.FindByIdAsync(id);


        if (entity != null)
        {
           
            return View(entity);

        }

        return View("Index");
    }


    [HttpPost]
     public async Task<ActionResult> DeleteConfirm(string id)
    {
        if (id == null)
        {

            return RedirectToAction("Index");
        }

        var entity = await _usermanager.FindByIdAsync(id);


        if (entity != null)
        {
            var result = await _usermanager.DeleteAsync(entity);
           
            if (result.Succeeded)
            {
               TempData["Mesaj"] = "Kullanıcı başarıyla silindi.";
            }
        }

        return RedirectToAction("Index");
    }


    
}