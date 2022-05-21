using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Motos.Web.Data;
using Motos.Web.Helpers;
using Motos.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Motos.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    [Authorize(Roles = "Admin")]

    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public ServiceController(ApplicationDbContext context, IBlobHelper blobHelper, ICombosHelper combosHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Services
                .Include(p => p.Category)
                .Include(p => p.ServiceImages)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ServiceViewModel model = new ServiceViewModel
            {
                Categories = _combosHelper.GetComboCategories(),
                IsActive = true
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Service service = await _converterHelper.ToServiceAsync(model, true);

                    if (model.ImageFile != null)
                    {
                        Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "services");
                        service.ServiceImages = new List<ServiceImage>
                {
                    new ServiceImage { ImageId = imageId }
                };
                    }

                    _context.Add(service);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Categories = _combosHelper.GetComboCategories();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service service = await _context.Services
                .Include(p => p.Category)
                .Include(p => p.ServiceImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            ServiceViewModel model = _converterHelper.ToServiceViewModel(service);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Service service = await _converterHelper.ToServiceAsync(model, false);

                    if (model.ImageFile != null)
                    {
                        Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "services");
                        if (service.ServiceImages == null)
                        {
                            service.ServiceImages = new List<ServiceImage>();
                        }

                        service.ServiceImages.Add(new ServiceImage { ImageId = imageId });
                    }

                    _context.Update(service);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Categories = _combosHelper.GetComboCategories();
            return View(model);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service service = await _context.Services
                .Include(p => p.ServiceImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            try
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service service = await _context.Services
                .Include(c => c.Category)
                .Include(c => c.ServiceImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }


        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            AddServiceImageViewModel model = new AddServiceImageViewModel { ServiceId = service.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddServiceImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Service service = await _context.Services
                    .Include(p => p.ServiceImages)
                    .FirstOrDefaultAsync(p => p.Id == model.ServiceId);
                if (service == null)
                {
                    return NotFound();
                }

                try
                {
                    Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "services");
                    if (service.ServiceImages == null)
                    {
                        service.ServiceImages = new List<ServiceImage>();
                    }

                    service.ServiceImages.Add(new ServiceImage { ImageId = imageId });
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = service.Id });

                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceImage serviceImage = await _context.ServicesImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceImage == null)
            {
                return NotFound();
            }

            Service service = await _context.Services.FirstOrDefaultAsync(p => p.ServiceImages.FirstOrDefault(pi => pi.Id == serviceImage.Id) != null);
            _context.ServicesImages.Remove(serviceImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = service.Id });
        }














    }
}
