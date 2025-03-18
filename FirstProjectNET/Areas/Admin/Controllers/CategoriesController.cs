using AutoMapper;
using FirstProjectNET.Data;
using FirstProjectNET.Models;
using FirstProjectNET.Models.Common;
using FirstProjectNET.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstProjectNET.Areas.Admin.Controllers
{       
    [Area("Admin")]
    [Route("Admin/Categories")]
    public class CategoriesController : Controller
    {
        private HotelDbContext _db;
        private readonly IMapper _mapper;

        public CategoriesController(HotelDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index(string SortColumn = "CategoryID", string IconClass = "fa-sort-asc", int page = 1)
        {
            var categories = _db.Categories.AsQueryable();
            // Sắp xếp
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;
            categories = SortRooms(categories, SortColumn, IconClass);

            // Phân trang
            int NoOfRecordPerPage = 5;
            int NoOfPages = (int)Math.Ceiling((double)categories.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            categories = categories.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            var categoryList = categories.ToList();
            var categoryViewModel = _mapper.Map<IEnumerable<AdminCategoryViewModel>>(categoryList);
            return View(categoryViewModel);
        }
        // Phương thức sắp xếp riêng trả về IQueryable
        private IQueryable<Category> SortRooms(IQueryable<Category> categories, string SortColumn, string IconClass)
        {
            if (SortColumn == "CategoryID")
            {
                categories = IconClass == "fa-sort-asc" ? categories.OrderBy(r => r.CategoryID) : categories.OrderByDescending(r => r.CategoryID);
            }
            else if (SortColumn == "Price")
            {
                categories = IconClass == "fa-sort-asc" ? categories.OrderBy(r => r.Price) : categories.OrderByDescending(r => r.Price);
            }
            else if (SortColumn == "TypeName")
            {
                categories = IconClass == "fa-sort-asc" ? categories.OrderBy(r => r.TypeName) : categories.OrderByDescending(r => r.TypeName);
            }else if(SortColumn == "Capacity")
            {
                categories = IconClass == "fa-sort-asc" ? categories.OrderBy(r => r.Capacity) : categories.OrderByDescending(r => r.Capacity);
            }
            return categories;
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            // Check role
            var roleString = HttpContext.Session.GetString("Role");
            if (Enum.TryParse(roleString, out AccountType role) && role != AccountType.Admin)
            {
                // Not permission
                return RedirectToAction("AccessDenied", "Account");
            }
            var model = new AdminCategoryViewModel
            {
                ListTypeName = new List<string> { "Standard Single", "Standard Twin", "Superior Single", "Superior Twin", "Superior Triple", "Deluxe Single", "Deluxe Twin", "Suite Single", "Suite Twin", "Suite Triple", "Suite Queen" }
            };
            //ViewBag.TypeName = new List<string> { "Standard Single", "Standard Twin", "Superior Single", "Superior Twin", "Superior Triple", "Deluxe Single", "Deluxe Twin", "Suite Single", "Suite Twin", "Suite Triple", "Suite Queen"};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public IActionResult Create(AdminCategoryViewModel model)
        {
            ModelState.Remove("ListTypeName");
            if (ModelState.IsValid)
            {
                Category oldCategory = _db.Categories.Find(model.CategoryID);
                if(oldCategory == null)
                {
                    var category = _mapper.Map<Category>(model);
                    _db.Categories.Add(category);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("CategoryID", "CategoryID đã tồn tại.");
                }
            }
            
            model.ListTypeName = new List<string> { "Standard Single", "Standard Twin", "Superior Single", "Superior Twin", "Superior Triple", "Deluxe Single", "Deluxe Twin", "Suite Single", "Suite Twin", "Suite Triple", "Suite Queen" };
            return View(model);
        }

        [Route("Edit")]
        public IActionResult Edit(string id)
        {
            // Check role
            var roleString = HttpContext.Session.GetString("Role");
            if (Enum.TryParse(roleString, out AccountType role) && role != AccountType.Admin)
            {
                // Not permission
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id == null || _db.Categories == null)
            {
                return NotFound();
            }
            var category = _db.Categories.Find(id);
            var categoryViewModel = _mapper.Map<AdminCategoryViewModel>(category);

            if(category == null)
            {
                return NotFound();
            }
            categoryViewModel.ListTypeName = new List<string> { "Standard Single", "Standard Twin", "Superior Single", "Superior Twin", "Superior Triple",
        "Deluxe Single", "Deluxe Twin", "Suite Single", "Suite Twin", "Suite Triple", "Suite Queen"};
            return View(categoryViewModel);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, AdminCategoryViewModel model)
        {
            ModelState.Remove("ListTypeName");
            if (ModelState.IsValid)
            {
                try
                {
                    var category = _mapper.Map<Category>(model);
                    _db.Update(category);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(model.CategoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            model.ListTypeName = new List<string>
            {
                "Standard Single", "Standard Twin", "Superior Single", "Superior Twin",
                "Superior Triple", "Deluxe Single", "Deluxe Twin", "Suite Single",
                "Suite Twin", "Suite Triple", "Suite Queen"
            };
            return View(model);
        }
        private bool CategoryExists(string id)
        {
            return (_db.Categories?.Any(e => e.CategoryID == id)).GetValueOrDefault();
        }

        [Route("Delete")]
        public IActionResult Delete(string id)
        {
            // Check role
            var roleString = HttpContext.Session.GetString("Role");
            if (Enum.TryParse(roleString, out AccountType role) && role != AccountType.Admin)
            {
                // Not permission
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id== null || _db.Categories == null)
            {
                return NotFound();
            }

            var category = _db.Categories.Include(r => r.Rooms).FirstOrDefault(c => c.CategoryID == id);

            if(category == null)
            {
                return NotFound();
            }

            if(category.Rooms.Count() > 0)
            {
                return Content("This Category has some rooms, can't delete!");

            }
            return View(category);
        }

        [HttpPost]
        [Route("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            if (_db.Categories == null)
            {
                return Problem("Entity set 'Categories' is null.");
            }
            var category = _db.Categories.Find(id);
            if(category != null)
            {
                _db.Categories.Remove(category);
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
