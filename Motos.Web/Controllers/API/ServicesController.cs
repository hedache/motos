using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motos.Web.Data;
using System.Linq;

namespace Motos.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]

    public class ServicesController : ControllerBase
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_context.Services
                .Include(p => p.Category)
                .Include(p => p.ServiceImages)
                .Where(p => p.IsActive));
        }

    }
}
