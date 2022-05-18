//using System;
//using Motos.Web.Models;
//namespace Motos.Web.Helpers
//{
//    public class ConverterHelper : IConverterHelper
//    {
//        public User ToUser(UserViewModel model, Guid imageId, bool isNew)
//        {
//            return new User
//            {
//                Id = isNew ? 0 : model.Id,
//                ImageId = imageId,
//                Name = model.Name
//            };
//        }

//        public UserViewModel ToUserViewModel(User user)
//        {
//            return new UserViewModel
//            {
//                Id = user.Id,
//                ImageId = user.ImageId,
//                Name = user.Name
//            };
//        }
//    }

//}
