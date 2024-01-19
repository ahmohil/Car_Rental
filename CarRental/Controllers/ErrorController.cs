using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("NotFound")]
        public IActionResult NotFoundError()
        {
            return View();
        }

        [Route("ServerError")]
        public IActionResult ServerError()
        {
            return View();
        }
    }
}
