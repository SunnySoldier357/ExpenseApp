using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    [Route("")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Admin Home Page
            return View();
        }

        [Route("accounts/list/")]
        public IActionResult List()
        {
            return View();
        }

        [Route("accounts/create/")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("accounts/edit/{name}")]
        public IActionResult Edit(string name)
        {
            return View();
        }

        [Route("accounts/delete/{name}")]
        public IActionResult Delete(string name)
        {
            return View();
        }
    }
}