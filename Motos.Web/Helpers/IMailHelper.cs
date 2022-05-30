using Swashbuckle.Swagger;

namespace Motos.Web.Helpers
{
    public interface IMailHelper
    {
Response         SendMail(string to, string subject, string body);
    }
}
