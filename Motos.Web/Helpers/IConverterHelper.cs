using Motos.Web.Models;
using System;
namespace Motos.Web.Helpers
{
    public interface IConverterHelper
    {
        User ToUser(UserViewModel model, Guid imageId, bool isNew);

        UserViewModel ToUserViewModel(User user);

    }
}
