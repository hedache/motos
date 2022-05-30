using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Motos.Web.Data;
using Motos.Web.Models;

public class CombosHelper : ICombosHelper
{
    private readonly ApplicationDbContext _context;

    public CombosHelper(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<SelectListItem> GetComboCategories()
    {
        List<SelectListItem> list = _context.Categories.Select(t => new SelectListItem
        {
            Text = t.Name,
            Value = $"{t.Id}"
        })
            .OrderBy(t => t.Text)
            .ToList();

        list.Insert(0, new SelectListItem
        {
            Text = "[Select a category...]",
            Value = "0"
        });

        return list;
    }

    //public IEnumerable<SelectListItem> GetComboRegistries(int personId)
    //{
    //    List<SelectListItem> list = new List<SelectListItem>();
    //    Person person = _context.Persons
    //        .Include(d => d.Registries)
    //        .FirstOrDefault(d => d.Id == departmentId);
    //    if (department != null)
    //    {
    //        list = department.Cities.Select(t => new SelectListItem
    //        {
    //            Text = t.Name,
    //            Value = $"{t.Id}"
    //        })
    //            .OrderBy(t => t.Text)
    //            .ToList();
    //    }

    //    list.Insert(0, new SelectListItem
    //    {
    //        Text = "[Select a city...]",
    //        Value = "0"
    //    });

    //    return list;
    //}

    //public IEnumerable<SelectListItem> GetComboCountries()
    //{
    //    List<SelectListItem> list = _context.Countries.Select(t => new SelectListItem
    //    {
    //        Text = t.Name,
    //        Value = $"{t.Id}"
    //    })
    //        .OrderBy(t => t.Text)
    //        .ToList();

    //    list.Insert(0, new SelectListItem
    //    {
    //        Text = "[Select a country...]",
    //        Value = "0"
    //    });

    //    return list;
    //}

    //public IEnumerable<SelectListItem> GetComboDepartments(int countryId)
    //{
    //    List<SelectListItem> list = new List<SelectListItem>();
    //    Country country = _context.Countries
    //        .Include(c => c.Departments)
    //        .FirstOrDefault(c => c.Id == countryId);
    //    if (country != null)
    //    {
    //        list = country.Departments.Select(t => new SelectListItem
    //        {
    //            Text = t.Name,
    //            Value = $"{t.Id}"
    //        })
    //            .OrderBy(t => t.Text)
    //            .ToList();
    //    }

    //    list.Insert(0, new SelectListItem
    //    {
    //        Text = "[Select a department...]",
    //        Value = "0"
    //    });

    //    return list;
    //}










}
