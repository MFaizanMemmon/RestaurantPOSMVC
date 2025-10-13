using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagementUI.Models;
using RestaurantManagementUI.Repository;
using RestaurantManagementUI.Unit_of_work;
using RestaurantManagementUI.View_Models;
using System.Globalization;
using System.Numerics;

namespace RestaurantManagementUI.Controllers
{
    public class PointOfSaleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PointOfSaleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult AdminDashboard()
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

        #region Category
        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var categories = await _unitOfWork.Categories.GetAllCategory();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAddEdit(int? id)
        {
            var category = new tbl_Category();

            if (id != null)
                category = await _unitOfWork.Categories.GetCategoryByID(Convert.ToInt32(id));

            // Return a Partial View (not a full View)
            return PartialView("_CategoryAddEdit", category);
        }


        [HttpPost]
        public async Task<IActionResult> CategoryCreate([FromBody] tbl_Category category)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new { success = false, message = "Validation failed.", errors });
                }

                _unitOfWork.BeginTransaction();

                if (category.CategoryID == 0)
                {
                    await _unitOfWork.Categories.AddCategory(category);
                    TempData["Success"] = "Category created successfully!";
                }
                else
                {
                    await _unitOfWork.Categories.UpdateCategory(category);
                    TempData["Success"] = "Category updated successfully!";
                }

                _unitOfWork.CommitTransaction();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
                return Json(new { success = false, message = ex.Message });
            }
        }






        public async Task<IActionResult> CategoryDelete(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                await _unitOfWork.Categories.DeleteCategory(id);
                _unitOfWork.CommitTransaction();

                TempData["Success"] = "Category deleted successfully!";
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("CategoryList");
        }
        #endregion

        #region Product

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var products = await _unitOfWork.Products.GetAllProduct();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ProductAddEdit(int? id)
        {
            var product = new PorductViewModel();



            if (id != null)
                product = await _unitOfWork.Products.GetProductByID(id);

            var categories = await _unitOfWork.Categories.GetAllCategory();
            product.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName
                })
                .ToList();


            return PartialView("_ProductAddEdit", product);
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(PorductViewModel product)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                string imageUrl = product.ImageUrl; // Keep old image if editing

                // ✅ Handle new image upload
                if (product.ProductUrl != null && product.ProductUrl.Length > 0)
                {
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(product.ProductUrl.FileName)}";
                    string filePath = Path.Combine(folderPath, fileName);

                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ProductUrl.CopyToAsync(stream);
                    }

                    imageUrl = "/images/products/" + fileName;
                }

                // ✅ Map ViewModel → Entity
                var entity = new tbl_Product
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    ProductPrice = (int)product.ProductPrice,
                    CategoryId = product.CategoryID,
                    ImageUrl = imageUrl
                };

                // ✅ Insert or Update
                if (product.ProductID == 0)
                {
                    await _unitOfWork.Products.AddProduct(entity);
                    TempData["Success"] = "Product created successfully!";
                }
                else
                {
                    await _unitOfWork.Products.UpdateProduct(entity);
                    TempData["Success"] = "Product updated successfully!";
                }

                _unitOfWork.CommitTransaction();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
                return Json(new { success = false, message = ex.Message });
            }
        }



        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                await _unitOfWork.Products.DeleteProduct(id);
                _unitOfWork.CommitTransaction();

                TempData["Success"] = "Product deleted successfully!";
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("ProductList");
        }

        #endregion

        #region Table
        [HttpGet]
        public async Task<IActionResult> TableList()
        {
            var table = await _unitOfWork.Table.GetAllTable();
            return View(table);
        }

        [HttpGet]
        public async Task<IActionResult> TableAddEdit(int? id)
        {
            var table = new tbl_Table();
            if (id != null)
                table = await _unitOfWork.Table.GetTableByID(Convert.ToInt32(id));

            return PartialView("_TableAddEdit", table);
        }
        [HttpPost]
        public async Task<IActionResult> TableAddEdit(tbl_Table table)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (table.Tid == 0)
                {
                    await _unitOfWork.Table.AddTable(table);
                    TempData["Success"] = "Table created successfully!";
                }
                else
                {
                    await _unitOfWork.Table.UpdateTable(table);
                    TempData["Success"] = "Table updated successfully!";
                }

                _unitOfWork.CommitTransaction();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTable(int? id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                await _unitOfWork.Table.DeleteTable(id);
                _unitOfWork.CommitTransaction();
                TempData["Success"] = "Table deleted successfully!";
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("TableList");
        }

        #endregion

        #region Staff
        [HttpGet]
        public async Task<IActionResult> StaffList()
        {
            var staff = await _unitOfWork.Staffs.GetAllStaff();
            return View(staff);
        }

        [HttpGet]
        public async Task<IActionResult> StaffAddEdit(int? id)
        {
            var staff = new StaffViewModel();
            var role = await _unitOfWork.Staffs.GetAllRoles();

            if (id != null)
                staff = await _unitOfWork.Staffs.GetStaffByID(Convert.ToInt32(id));

            staff.Roles = role
               .Select(r => new SelectListItem
               {
                   Value = r.RoleID.ToString(),
                   Text = r.RoleName
               })
               .ToList();
            return PartialView("_StaffAddEdit", staff);
        }
        [HttpPost]
        public async Task<IActionResult> StaffAddEdit([FromBody] StaffViewModel staff)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (staff.StaffID == 0)
                {
                    await _unitOfWork.Staffs.AddStaff(staff);
                    TempData["Success"] = "Staff created successfully!";
                }
                else
                {
                    await _unitOfWork.Staffs.UpdateStaff(staff);
                    TempData["Success"] = "Staff updated successfully!";
                }

                _unitOfWork.CommitTransaction();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;

                return Json(new { success = false });
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteStaff(int? id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                await _unitOfWork.Staffs.DeleteStaff(id);
                _unitOfWork.CommitTransaction();
                TempData["Success"] = "Staff deleted successfully!";
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("StaffList");
        }





        #endregion

        #region POS Section

        public async Task<ActionResult> GetAllOrders()
        {
            GetAllOrdersViewModel getAllOrders = new GetAllOrdersViewModel();
            getAllOrders.OrderHeader = await _unitOfWork.POS.GetAllOrders();
            getAllOrders.OrderDetails = await _unitOfWork.POS.GetAllOrderDetail();
            return View(getAllOrders);
        }


        public async Task<IActionResult> AdminPOS(int? id)
        {
            var model = new AdminPOSViewModel();

            // Get all products
            var products = await _unitOfWork.Products.GetAllProduct();

            // Map products
            model.Products = products?
                .Select(p => new tbl_Product
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    CategoryId = p.CategoryID,
                    ImageUrl = p.ImageUrl
                })
                .ToList() ?? new List<tbl_Product>();

            // Map staff
            var staffList = await _unitOfWork.Staffs.GetAllStaff() ?? Enumerable.Empty<tbl_Staff>();
            model.Staff = staffList.Cast<tbl_Staff?>().ToList();

            // Map categories
            var categoryList = await _unitOfWork.Categories.GetAllCategory() ?? Enumerable.Empty<tbl_Category>();
            model.Categories = categoryList.Cast<tbl_Category?>().ToList();

            // Map tables
            var tableList = await _unitOfWork.Table.GetAllTable() ?? Enumerable.Empty<tbl_Table>();
            model.Tables = tableList.Cast<tbl_Table?>().ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReceivedPayment([FromBody] tbl_Main header)
        {
            if (header == null || header.MainID == 0)
                return BadRequest(new { success = false, message = "Invalid order header data." });

            try
            {
                var updatedId = await _unitOfWork.POS.UpdateReceivedPayment(
                    header.MainID,
                    header.Recieved,
                    header.Change,
                    header.MasterConfigParentID
                );

                if (updatedId > 0)
                    return Json(new { success = true, message = "Payment received successfully!", id = updatedId });
                else
                    return Json(new { success = false, message = "Payment update failed." });
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }



        public async Task<IActionResult> ReceivedPayment(int id)
        {
            ReceivedPaymentViewModel receivedPaymentViewModel = new ReceivedPaymentViewModel();
            var masterConfig = await _unitOfWork.MasterConfig.GetAllMasterConfigByParentID(1);


            receivedPaymentViewModel.PaymentTypeList = masterConfig
                .Select(mc => new SelectListItem
                {
                    Value = mc.ConfigID.ToString(),
                    Text = mc.ConfigName
                })
                .ToList();

            tbl_Main main = new tbl_Main();
            main = await _unitOfWork.POS.GetOrderHeaderByID(id);

            receivedPaymentViewModel.MainID = main.MainID;
            receivedPaymentViewModel.TotalAmount = main.Total;

            return View("_ReceivedPayment", receivedPaymentViewModel);
        }



        public async Task<IActionResult> WaiterOrderTaking()
        {
            var model = new AdminPOSViewModel();

            // Get all products
            var products = await _unitOfWork.Products.GetAllProduct();

            // Map products
            model.Products = products?
                .Select(p => new tbl_Product
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    CategoryId = p.CategoryID,
                    ImageUrl = p.ImageUrl
                })
                .ToList() ?? new List<tbl_Product>();

            // Map staff
            var staffList = await _unitOfWork.Staffs.GetAllStaff() ?? Enumerable.Empty<tbl_Staff>();
            model.Staff = staffList.Cast<tbl_Staff?>().ToList();

            // Map categories
            var categoryList = await _unitOfWork.Categories.GetAllCategory() ?? Enumerable.Empty<tbl_Category>();
            model.Categories = categoryList.Cast<tbl_Category?>().ToList();

            // Map tables
            var tableList = await _unitOfWork.Table.GetAllTable() ?? Enumerable.Empty<tbl_Table>();
            model.Tables = tableList.Cast<tbl_Table?>().ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] ConfirmOrderViewModel order)
        {
            try
            {

                _unitOfWork.BeginTransaction();
                int mainId = await _unitOfWork.POS.AddOrderHeader(order.OrderHeader);


                foreach (var detail in order.OrderDetail)
                {
                    detail.MainID = mainId;
                }


                await _unitOfWork.POS.AddOrderDetail(order.OrderDetail);

                _unitOfWork.CommitTransaction();
                TempData["Success"] = "Order has been created!";
                return Json(new { success = true, message = "Order created successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public async Task<ActionResult> KOT()
        {
            GetAllOrdersViewModel getAllOrders = new GetAllOrdersViewModel();
            getAllOrders.OrderHeader = await _unitOfWork.POS.GetAllOrders();
            getAllOrders.OrderDetails = await _unitOfWork.POS.GetAllOrderDetail();
            return View(getAllOrders);
        }

        #endregion

    }
}








