using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Motos.Web.Data;
using Motos.Web.Models;
using Motos.Web.Helpers;

namespace Motos.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly IBlobHelper _blobHelper;
        //private readonly IConverterHelper _converterHelper;


        public RolesController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.Include(c => c.Users).ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .Include(c => c.Users).ThenInclude(d => d.Registries)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(role);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "hay un registro con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                            dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(role);
        }

        //GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Role role)
        {
            if (id != role.Id) { return NotFound(); }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "hay un registro con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                            dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(role);
        }




        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Role role)
        //{
        //    if (id != role.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(role);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RoleExists(role.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(role);
        //}

        // GET: Roles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            Role role = await _context.Roles
                .Include(c => c.Users)
                .ThenInclude(d => d.Registries)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null) { return NotFound(); }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // POST: Roles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Roles == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Roles'  is null.");
        //    }
        //    var role = await _context.Roles.FindAsync(id);
        //    if (role != null)
        //    {
        //        _context.Roles.Remove(role);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool RoleExists(int id)
        //{
        //    return _context.Roles.Any(e => e.Id == id);
        //}

        public async Task<IActionResult> AddUser(int? id)
        {
            if (id == null) { return NotFound(); }
            Role role = await _context.Roles.FindAsync(id); if (role == null) { return NotFound(); }
            User model = new User { IdRol = role.Id }; return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                Role role = await _context.Roles.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == user.IdRol); if (role == null) { return NotFound(); }
                try
                {
                    user.Id = 0; role.Users.Add(user); _context.Update(role); await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = role.Id });
                }
                catch (DbUpdateException dbUpdateException) { if (dbUpdateException.InnerException.Message.Contains("duplicate")) { ModelState.AddModelError(string.Empty, "There are a record with the same name."); } else { ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message); } }
                catch (Exception exception) { ModelState.AddModelError(string.Empty, exception.Message); }
            }
            return View(user);
        }

        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null) { return NotFound(); }
            User user = await _context.Users.FindAsync(id); if (user == null) { return NotFound(); }
            Role role = await _context.Roles.FirstOrDefaultAsync(c => c.Users.FirstOrDefault(d => d.Id == user.Id) != null); user.IdRol = role.Id; return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Details), new { Id = user.IdRol });
                }
                catch (DbUpdateException dbUpdateException) { if (dbUpdateException.InnerException.Message.Contains("duplicate")) { ModelState.AddModelError(string.Empty, "There are a record with the same name."); } else { ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message); } }
                catch (Exception exception)
                {

                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(user);
        }



        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null) { return NotFound(); }
            User user = await _context.Users.Include(d => d.Registries).FirstOrDefaultAsync(m => m.Id == id); if (user == null) { return NotFound(); }
            Role role = await _context.Roles.FirstOrDefaultAsync(c => c.Users.FirstOrDefault(d => d.Id == user.Id) != null); _context.Users.Remove(user); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Details), new { Id = role.Id });
        }

        public async Task<IActionResult> DetailsUser(int? id)
        {
            if (id == null) { return NotFound(); }
            User user = await _context.Users.Include(d => d.Registries).FirstOrDefaultAsync(m => m.Id == id); if (user == null) { return NotFound(); }
            Role role = await _context.Roles.FirstOrDefaultAsync(c => c.Users.FirstOrDefault(d => d.Id == user.Id) != null); user.IdRol = role.Id; return View(user);
        }





        public async Task<IActionResult> AddRegistry(int? id)
        {
            if (id == null) { return NotFound(); }
            User user = await _context.Users.FindAsync(id); if (user == null) { return NotFound(); }
            Registry model = new Registry { IdUser = user.Id }; return View(model);
            //UserViewModel model = new UserViewModel();
            //return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRegistry(Registry registry)

        {
            //double cosa = Convert.ToDouble(registry.Costx);
            //registry.Costx = cosa;
            if (ModelState.IsValid)
            {
                User user = await _context.Users.Include(d => d.Registries).FirstOrDefaultAsync(c => c.Id == registry.IdUser); if (user == null) { return NotFound(); }
                try
                {
                    registry.Id = 0; user.Registries.Add(registry); _context.Update(user); await _context.SaveChangesAsync(); return RedirectToAction(nameof(DetailsUser), new { Id = user.Id });
                }
                catch (DbUpdateException dbUpdateException) { if (dbUpdateException.InnerException.Message.Contains("duplicate")) { ModelState.AddModelError(string.Empty, "There are a record with the same name."); } else { ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message); } }
                catch (Exception exception) { ModelState.AddModelError(string.Empty, exception.Message); }
            }
            return View(registry);

            //if (ModelState.IsValid)
            //{
            //    Guid imageId = Guid.Empty;

            //    if (model.ImageFile != null)
            //    {
            //        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
            //    }

            //    try
            //    {
            //        User user = _converterHelper.ToUser(model, imageId, true);
            //        _context.Add(user);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    catch (DbUpdateException dbUpdateException)
            //    {
            //        if (dbUpdateException.InnerException.Message.Contains("duplicate"))
            //        {
            //            ModelState.AddModelError(string.Empty, "There are a record with the same name.");
            //        }
            //        else
            //        {
            //            ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
            //        }
            //    }
            //    catch (Exception exception)
            //    {
            //        ModelState.AddModelError(string.Empty, exception.Message);
            //    }
            //}

            //return View(model);
        }
















    public async Task<IActionResult> EditRegistry(int? id)
    {
        if (id == null) { return NotFound(); }
        Registry registry = await _context.Registries.FindAsync(id); if (registry == null) { return NotFound(); }
        User user = await _context.Users.FirstOrDefaultAsync(d => d.Registries.FirstOrDefault(c => c.Id == registry.Id) != null); registry.IdUser = user.Id; return View(registry);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRegistry(Registry registry)
    {
        if (ModelState.IsValid) { try { _context.Update(registry); await _context.SaveChangesAsync(); return RedirectToAction(nameof(DetailsUser), new { Id = registry.IdUser }); } catch (DbUpdateException dbUpdateException) { if (dbUpdateException.InnerException.Message.Contains("duplicate")) { ModelState.AddModelError(string.Empty, "There are a record with the same name."); } else { ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message); } } catch (Exception exception) { ModelState.AddModelError(string.Empty, exception.Message); } }
        return View(registry);

    }



    public async Task<IActionResult> DeleteRegistry(int? id)
    {
        if (id == null) { return NotFound(); }
        Registry registry = await _context.Registries.FirstOrDefaultAsync(m => m.Id == id); if (registry == null) { return NotFound(); }
        User user = await _context.Users.FirstOrDefaultAsync(d => d.Registries.FirstOrDefault(c => c.Id == registry.Id) != null); _context.Registries.Remove(registry); await _context.SaveChangesAsync();
        return RedirectToAction(nameof(DetailsUser), new { Id = user.Id });
    }










    }
}
