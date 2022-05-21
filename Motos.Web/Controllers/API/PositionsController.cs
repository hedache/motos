using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motos.Web.Data;



namespace Motos.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]

    public class PositionsController : ControllerBase
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPositions()
        {
            return Ok(_context.Positions
                .Include(c => c.Persons)
                .ThenInclude(d => d.Registries));
        }

    }
}
