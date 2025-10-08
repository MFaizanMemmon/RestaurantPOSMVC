using Microsoft.AspNetCore.Mvc;
using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Login()
        {
           
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "admin" && model.Password == "1234")
                {
                    // Save username in session
                    HttpContext.Session.SetString("Username", model.Username);

                    // Redirect to Home/Dashboard
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    model.ErrorMessage = "Invalid username or password!";
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

