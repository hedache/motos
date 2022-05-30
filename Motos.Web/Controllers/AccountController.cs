using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Motos.Web.Helpers;
using Motos.Web.Models;
using Motos.Web.Data;
using Motos.Web.Enums;
using System;
using Swashbuckle.Swagger;
using Microsoft.AspNetCore.Identity;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IUserHelper _userHelper;
    private readonly ICombosHelper _combosHelper;
    private readonly IBlobHelper _blobHelper;
    private readonly object _mailHelper;

    public AccountController(
        ApplicationDbContext context,
        IUserHelper userHelper,
        ICombosHelper combosHelper,
        IBlobHelper blobHelper
        )

    {
         _context = context;
        _userHelper = userHelper;
        _combosHelper = combosHelper;
        _blobHelper = blobHelper;

    }

public IActionResult NotAuthorized()
    {
        return View();
    }


    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new LoginViewModel());
    }




    public IActionResult Register()
    {
        AddUserViewModel model = new AddUserViewModel
        {
        //    Countries = _combosHelper.GetComboCountries(),
        //    Departments = _combosHelper.GetComboDepartments(0),
        //    Cities = _combosHelper.GetComboCities(0),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(AddUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            Guid imageId = Guid.Empty;

            if (model.ImageFile != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
            }


            User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "This email is already used.");
                //model.Countries = _combosHelper.GetComboCountries();
                //model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
                //model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
                return View(model);
            }

            //string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            //string tokenLink = Url.Action("ConfirmEmail", "Account", new
            //{
            //    userid = user.Id,
            //    token = myToken
            //}, protocol: HttpContext.Request.Scheme);

            //Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
            //    $"To allow the user, " +
            //    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
            //if (response.IsSuccess)
            //{
            //    ViewBag.Message = "The instructions to allow your user has been sent to email.";
            //    return View(model);
            //}

            //ModelState.AddModelError(string.Empty, response.Message);



            LoginViewModel loginViewModel = new LoginViewModel
            {
                Password = model.Password,
                RememberMe = false,
                Username = model.Username
            };

            var result2 = await _userHelper.LoginAsync(loginViewModel);

            if (result2.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //model.Countries = _combosHelper.GetComboCountries();
        //model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
        //model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
        return View(model);
    }


    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            return NotFound();
        }

        User user = await _userHelper.GetUserAsync(new Guid(userId));
        if (user == null)
        {
            return NotFound();
        }

        IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            return NotFound();
        }

        return View();
    }


    public async Task<IActionResult> ChangeUser()
    {
        User user = await _userHelper.GetUserAsync(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }

        //Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Cities.FirstOrDefault(c => c.Id == user.City.Id) != null);
        //if (department == null)
        //{
        //    department = await _context.Departments.FirstOrDefaultAsync();
        //}

        //Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.Id == department.Id) != null);
        //if (country == null)
        //{
        //    country = await _context.Countries.FirstOrDefaultAsync();
        //}

        EditUserViewModel model = new EditUserViewModel
        {
            Address = user.Address,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            ImageId = user.ImageId,
            //Cities = _combosHelper.GetComboCities(department.Id),
            //CityId = user.City.Id,
            //Countries = _combosHelper.GetComboCountries(),
            //CountryId = country.Id,
            //DepartmentId = department.Id,
            //Departments = _combosHelper.GetComboDepartments(country.Id),
            Id = user.Id,
            Document = user.Document
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeUser(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            Guid imageId = model.ImageId;

            if (model.ImageFile != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.ImageId = imageId;
            //user.City = await _context.Cities.FindAsync(model.CityId);
            user.Document = model.Document;

            await _userHelper.UpdateUserAsync(user);
            return RedirectToAction("Index", "Home");
        }

        //model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
        //model.Countries = _combosHelper.GetComboCountries();
        //model.Departments = _combosHelper.GetComboDepartments(model.CityId);
        return View(model);
    }



    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("ChangeUser");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User no found.");
            }
        }

        return View(model);
    }





    //public JsonResult GetDepartments(int countryId)
    //{
    //    Country country = _context.Countries
    //        .Include(c => c.Departments)
    //        .FirstOrDefault(c => c.Id == countryId);
    //    if (country == null)
    //    {
    //        return null;
    //    }

    //    return Json(country.Departments.OrderBy(d => d.Name));
    //}

    //public JsonResult GetCities(int departmentId)
    //{
    //    Department department = _context.Departments
    //        .Include(d => d.Cities)
    //        .FirstOrDefault(d => d.Id == departmentId);
    //    if (department == null)
    //    {
    //        return null;
    //    }

    //    return Json(department.Cities.OrderBy(c => c.Name));
    //}








    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
            if (result.Succeeded)
            {
                if (Request.Query.Keys.Contains("ReturnUrl"))
                {
                    return Redirect(Request.Query["ReturnUrl"].First());
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Email or password incorrect.");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _userHelper.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}
