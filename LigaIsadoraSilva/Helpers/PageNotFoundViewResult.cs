using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LigaIsadoraSilva.Helpers
{
    public class PageNotFoundViewResult: ViewResult
    {
        public PageNotFoundViewResult(string viewName) 
        {
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
