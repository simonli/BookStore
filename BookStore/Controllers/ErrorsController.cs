using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class ErrorsController : Controller
    {
        private IHostingEnvironment _env;
        public ErrorsController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Route("[controller]/{statusCode}")]
        public IActionResult CustomError(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("~/Views/Errors/Error404.cshtml");
            }
            return View("~/Views/Errors/Error500.cshtml");
        }
//
//        public IActionResult CustomError2(int statusCode)
//        {
//            var filePath = $"{_env.WebRootPath}/errors/{(statusCode == 404 ? 404 : 500)}.html";
//            return new PhysicalFileResult(filePath, new MediaTypeHeaderValue("text/html"));
//        }
    }
}