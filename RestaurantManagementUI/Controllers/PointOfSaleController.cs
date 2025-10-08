using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagementUI.Models;
using RestaurantManagementUI.Repository;
using RestaurantManagementUI.Unit_of_work;
using RestaurantManagementUI.View_Models;

namespace RestaurantManagementUI.Controllers
{
    public class PointOfSaleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PointOfSaleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
        public async Task<IActionResult> CategoryCreate(tbl_Category category)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("CategoryList");
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
          
            var categories = await _unitOfWork.Categories.GetAllCategory();

            product.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName
                })
                .ToList();

            if (id != null)
                product = await _unitOfWork.Products.GetProductByID(id);
         
            return PartialView("_ProductAddEdit", product);
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(PorductViewModel product)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                string imageUrl = product.ImageUrl; // keep existing if edit

                // ✅ Handle new image upload
                if (product.ProductUrl != null && product.ProductUrl.Length > 0)
                {
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(product.ProductUrl.FileName)}";
                    string filePath = Path.Combine(folderPath, fileName);

                    // Delete old image (if any)
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    // Save new file
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
                    ProductPrice = (int)product.Price,
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
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("ProductList");
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
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("TableList");
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
    }
}


