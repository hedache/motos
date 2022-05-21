using System;
using System.Threading.Tasks;
using Motos.Web.Models;

public interface IConverterHelper
{
    Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew);

    CategoryViewModel ToCategoryViewModel(Category category);
    Task<Service> ToServiceAsync(ServiceViewModel model, bool isNew);

    ServiceViewModel ToServiceViewModel(Service service);

}
