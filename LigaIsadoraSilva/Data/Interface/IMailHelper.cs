using LigaIsadoraSilva.Helpers;
using System.Threading.Tasks;

namespace LigaIsadoraSilva.Data.Interface
{
    public interface IMailHelper
    {
        Task<Response> SendEmail(string to, string subject, string body);
    }
}