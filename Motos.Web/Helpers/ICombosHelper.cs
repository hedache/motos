using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


public interface ICombosHelper
{
    IEnumerable<SelectListItem> GetComboCategories();
    //IEnumerable<SelectListItem> GetComboPositions();

    //IEnumerable<SelectListItem> GetComboPersons(int countryId);

    //IEnumerable<SelectListItem> GetComboRegistries(int departmentId);

}
