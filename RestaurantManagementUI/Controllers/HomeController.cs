using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementUI.Models;

namespace RestaurantManagementUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.TotalOrders = 120;
        ViewBag.RevenueToday = 950.50;
        ViewBag.ActiveTables = 15;
        ViewBag.PendingOrders = 8;

        // Recent orders (sample list)
        ViewBag.RecentOrders = new List<dynamic>
            {
                new { OrderID = 101, TableNo = 5, Items = "Pizza, Coke", Total = 25.50, Status = "Served", OrderTime = DateTime.Now.AddMinutes(-30) },
                new { OrderID = 102, TableNo = 2, Items = "Burger, Fries", Total = 15.00, Status = "Pending", OrderTime = DateTime.Now.AddMinutes(-15) },
                new { OrderID = 103, TableNo = 7, Items = "Pasta", Total = 12.50, Status = "Served", OrderTime = DateTime.Now.AddMinutes(-10) },
            };

        // Orders and Revenue last 7 days
        ViewBag.OrdersLast7Days = new
        {
            Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
            Data = new[] { 15, 18, 12, 20, 22, 25, 30 }
        };

        ViewBag.RevenueLast7Days = new
        {
            Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
            Data = new[] { 150, 180, 120, 200, 220, 250, 300 }
        };
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Dashboard()
    {
        return View();
    }
    public IActionResult Category()
    {
        return View();
    }
    public IActionResult Product()
    {
        return View();
    }
    public IActionResult Table()
    {
        return View();
    }
    public IActionResult Staff()
    {
        return View();
    }
    public IActionResult POS()
    {
        return View();
    }
    public IActionResult Reports()
    {
        return View();
    }
}
