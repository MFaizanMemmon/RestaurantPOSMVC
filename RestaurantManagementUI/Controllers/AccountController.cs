using Microsoft.AspNetCore.Mvc;
using RestaurantManagementUI.Models;
using RestaurantManagementUI.Unit_of_work;

namespace RestaurantManagementUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Login()
        {
           
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

           
          
            tbl_Staff? staff = await _unitOfWork.Staffs.StaffLogin(model.Username!, model.Password!);
            if (staff != null)
            {
              
                HttpContext.Session.SetString("Username", staff.StaffName ?? staff.UserName ?? "");
                HttpContext.Session.SetInt32("StaffID", staff.StaffID);
                HttpContext.Session.SetInt32("RoleID", staff.RoleID);

               
                return RedirectToAction("AdminDashboard", "PointOfSale");
            }

            // Invalid login
            model.ErrorMessage = "Invalid username or password!";
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

