using Azure;

namespace LigaIsadoraSilva.Data.Interface
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
    }
}