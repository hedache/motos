using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Motos.Web.Models;
using System;
using Motos.Web.Enums;

namespace Motos.Web.Data
{
    public class SeedDb
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;
        public SeedDb(ApplicationDbContext context, IUserHelper userHelper) { 
            _context = context; 
            _userHelper = userHelper;
        }
        public async Task SeedAsync() { 
            await _context.Database.EnsureCreatedAsync(); 
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Orlando", "A", "oralpez@hotmail.com", "3000000000", "Calle Luna Calle Sol", UserType.Admin);
        
        }

    private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }


        private async Task<User> CheckUserAsync(
        string document,
        string firstName,
        string lastName,
        string email,
        string phone,
        string address,
        UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    Registry = _context.Registries.FirstOrDefault(),
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Positions.Any())
            {
                _context.Positions.Add(new Position
                {
                    Name = "Adminss",
                    Persons = new List<Person> { new Person { Dni = "123", Name = "Prueba1", Status=true, Registries = new List<Registry> { new Registry { Date = DateTime.Now, Dni="124", Placa="PRE9843", Owner="Juan", Description="Falla motor", Status="Pendiente" }, new Registry { Date = DateTime.Now, Dni = "1243232", Placa = "PRE984434", Owner = "Juan43", Description = "Falla motor", Status = "Pendiente" }, new Registry { Date = DateTime.Now, Dni = "1243434", Placa = "PRE943434384", Owner = "434343", Description = "Falla motor", Status = "Pendiente" } } }, new Person { Dni = "12423423434", Name = "4324", Status=true, Registries = new List<Registry>
{ new Registry { Date = DateTime.Now, Dni="1244234", Placa="654654", Owner="Juan6546", Description="Falla motor", Status="Pendiente" } } }, new Person { Dni = "12343333", Name = "Prueba15555", Status=true, Registries = new List<Registry> { new Registry { Date = DateTime.Now, Dni = "65645", Placa = "756756", Owner = "Juan00", Description = "Falla motor", Status = "Pendiente" }, new Registry { Date = DateTime.Now, Dni = "76647675", Placa = "76547457", Owner = "Juan66", Description = "Falla motor", Status = "Pendiente" }, new Registry { Date = DateTime.Now, Dni = "657567567", Placa = "7567666", Owner = "Juan776", Description = "Falla motor", Status = "Pendiente" } } } }
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
