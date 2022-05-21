using System;
using System.Threading.Tasks;
using Motos.Web.Data;
using Motos.Web.Helpers;
using Motos.Web.Models;

public class ConverterHelper : IConverterHelper
{
    public Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew)
    {
        return new Category
        {
            Id = isNew ? 0 : model.Id,
            ImageId = imageId,
            Name = model.Name
        };
    }

    public CategoryViewModel ToCategoryViewModel(Category category)
    {
        return new CategoryViewModel
        {
            Id = category.Id,
            ImageId = category.ImageId,
            Name = category.Name
        };
    }

    private readonly ApplicationDbContext _context;
    private readonly ICombosHelper _combosHelper;
    public ConverterHelper(ApplicationDbContext context, ICombosHelper combosHelper)
    {
        _context = context;
        _combosHelper = combosHelper;
    }

    public async Task<Service> ToServiceAsync(ServiceViewModel model, bool isNew)
    {
        return new Service
        {
            Category = await _context.Categories.FindAsync(model.CategoryId),
            Description = model.Description,
            Id = isNew ? 0 : model.Id,
            IsActive = model.IsActive,
            IsStarred = model.IsStarred,
            Name = model.Name,
            Price = model.Price,
            ServiceImages = model.ServiceImages
        };
    }

    public ServiceViewModel ToServiceViewModel(Service service)
    {
        return new ServiceViewModel
        {
            Categories = _combosHelper.GetComboCategories(),
            Category = service.Category,
            CategoryId = service.Category.Id,
            Description = service.Description,
            Id = service.Id,
            IsActive = service.IsActive,
            IsStarred = service.IsStarred,
            Name = service.Name,
            Price = service.Price,
            ServiceImages = service.ServiceImages
        };
    }


}
