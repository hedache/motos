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
    using Microsoft.AspNetCore.Authorization;

    [Authorize(Roles = "Admin")]

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
            return View(await _context.Positions.Include(c => c.Persons).ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Positions
                .Include(c => c.Persons).ThenInclude(d => d.Registries)
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
        public async Task<IActionResult> Create([Bind("Id,Name")] Position role)
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
        public async Task<IActionResult> Edit(int id, Position role)
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

            Position position = await _context.Positions
                .Include(c => c.Persons)
                .ThenInclude(d => d.Registries)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null) { return NotFound(); }
            _context.Positions.Remove(position);
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

        public async Task<IActionResult> AddPerson(int? id)
        {
            if (id == null) { return NotFound(); }
            Position role = await _context.Positions.FindAsync(id); if (role == null) { return NotFound(); }
            Person model = new Person { IdRol = role.Id }; return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPerson(Person person)
        {
            if (ModelState.IsValid)
            {
                Position role = await _context.Positions.Include(c => c.Persons).FirstOrDefaultAsync(c => c.Id == person.IdRol); if (role == null) { return NotFound(); }
                try
                {
                    person.Id = 0; role.Persons.Add(person); _context.Update(role); await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = role.Id });
                }
                catch (DbUpdateException dbUpdateException) { if (dbUpdateException.InnerException.Message.Contains("duplicate")) { ModelState.AddModelError(string.Empty, "There are a record with the same name."); } else { ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message); } }
                catch (Exception exception) { ModelState.AddModelError(string.Empty, exception.Message); }
            }
            return View(person);
        }

        public async Task<IActionResult> EditPerson(int? id)
        {
            if (id == null) { return NotFound(); }
            Person person = await _context.Persons.FindAsync(id); if (person == null) { return NotFound(); }
            Position role = await _context.Positions.FirstOrDefaultAsync(c => c.Persons.FirstOrDefault(d => d.Id == person.Id) != null); person.IdRol = role.Id; return View(person);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPerson(Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Details), new { Id = person.IdRol });
                }
                catch (DbUpdateException dbUpdateException) { if (dbUpdateException.InnerException.Message.Contains("duplicate")) { ModelState.AddModelError(string.Empty, "There are a record with the same name."); } else { ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message); } }
                catch (Exception exception)
                {

                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(person);
        }



        public async Task<IActionResult> DeletePerson(int? id)
        {
            if (id == null) { return NotFound(); }
            Person person = await _context.Persons.Include(d => d.Registries).FirstOrDefaultAsync(m => m.Id == id); if (person == null) { return NotFound(); }
            Position role = await _context.Positions.FirstOrDefaultAsync(c => c.Persons.FirstOrDefault(d => d.Id == person.Id) != null); _context.Persons.Remove(person); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Details), new { Id = role.Id });
        }

        public async Task<IActionResult> DetailsPerson(int? id)
        {
            if (id == null) { return NotFound(); }
            Person person = await _context.Persons.Include(d => d.Registries).FirstOrDefaultAsync(m => m.Id == id); if (person == null) { return NotFound(); }
            Position role = await _context.Positions.FirstOrDefaultAsync(c => c.Persons.FirstOrDefault(d => d.Id == person.Id) != null); person.IdRol = role.Id; return View(person);
        }





        public async Task<IActionResult> AddRegistry(int? id)
        {
            if (id == null) { return NotFound(); }
            Person person = await _context.Persons.FindAsync(id); if (person == null) { return NotFound(); }
            Registry model = new Registry { IdUser = person.Id }; return View(model);
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
                Person person = await _context.Persons.Include(d => d.Registries).FirstOrDefaultAsync(c => c.Id == registry.IdUser); if (person == null) { return NotFound(); }
                try
                {
                    registry.Id = 0; person.Registries.Add(registry); _context.Update(person); await _context.SaveChangesAsync(); return RedirectToAction(nameof(DetailsPerson), new { Id = person.Id });
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
        Person person = await _context.Persons.FirstOrDefaultAsync(d => d.Registries.FirstOrDefault(c => c.Id == registry.Id) != null); registry.IdUser = person.Id; return View(registry);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRegistry(Registry registry)
    {
        if (ModelState.IsValid) { try { _context.Update(registry); await _context.SaveChangesAsync(); return RedirectToAction(nameof(DetailsPerson), new { Id = registry.IdUser }); } catch (DbUpdateException dbUpdateException) { if (dbUpdateException.InnerException.Message.Contains("duplicate")) { ModelState.AddModelError(string.Empty, "There are a record with the same name."); } else { ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message); } } catch (Exception exception) { ModelState.AddModelError(string.Empty, exception.Message); } }
        return View(registry);

    }



    public async Task<IActionResult> DeleteRegistry(int? id)
    {
        if (id == null) { return NotFound(); }
        Registry registry = await _context.Registries.FirstOrDefaultAsync(m => m.Id == id); if (registry == null) { return NotFound(); }
        Person person = await _context.Persons.FirstOrDefaultAsync(d => d.Registries.FirstOrDefault(c => c.Id == registry.Id) != null); _context.Registries.Remove(registry); await _context.SaveChangesAsync();
        return RedirectToAction(nameof(DetailsPerson), new { Id = person.Id });
    }










    }
}
