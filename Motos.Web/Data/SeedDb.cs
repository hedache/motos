using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Motos.Web.Models;
using System;
namespace Motos.Web.Data
{
    public class SeedDb
    {
        private readonly ApplicationDbContext _context;
        public SeedDb(ApplicationDbContext context) { _context = context; }
        public async Task SeedAsync() { await _context.Database.EnsureCreatedAsync(); await CheckCountriesAsync(); }
        private async Task CheckCountriesAsync()
        {
            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role
                {
                    Name = "Admin",
                    Users = new List<User> { new User { Dni = "123", Name = "Prueba1", Status=true, Registries = new List<Registry> { new Registry { Date = DateTime.Now, Dni="124", Placa="PRE9843", Owner="Juan", Description="Falla motor", Status="Pendiente" }, new Registry { Date = DateTime.Now, Dni = "1243232", Placa = "PRE984434", Owner = "Juan43", Description = "Falla motor", Status = "Pendiente" }, new Registry { Date = DateTime.Now, Dni = "1243434", Placa = "PRE943434384", Owner = "434343", Description = "Falla motor", Status = "Pendiente" } } }, new User { Dni = "12423423434", Name = "4324", Status=true, Registries = new List<Registry>
{ new Registry { Date = DateTime.Now, Dni="1244234", Placa="654654", Owner="Juan6546", Description="Falla motor", Status="Pendiente" } } }, new User { Dni = "12343333", Name = "Prueba15555", Status=true, Registries = new List<Registry> { new Registry { Date = DateTime.Now, Dni = "65645", Placa = "756756", Owner = "Juan00", Description = "Falla motor", Status = "Pendiente" }, new Registry { Date = DateTime.Now, Dni = "76647675", Placa = "76547457", Owner = "Juan66", Description = "Falla motor", Status = "Pendiente" }, new Registry { Date = DateTime.Now, Dni = "657567567", Placa = "7567666", Owner = "Juan776", Description = "Falla motor", Status = "Pendiente" } } } }
                }); 
                 await _context.SaveChangesAsync();
            }
        }
    }
}
