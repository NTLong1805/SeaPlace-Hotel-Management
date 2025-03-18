using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstProjectNET.Data;
using FirstProjectNET.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using FirstProjectNET.Models.Common;
using FirstProjectNET.Models.ViewModel;
using AutoMapper;

namespace FirstProjectNET.Controllers
{
    [Area("Admin")]
    [Route("Admin/Booking")]
    public class BookingController : Controller
    {
        private readonly HotelDbContext _db;
        private readonly IMapper _mapper;
        public BookingController(HotelDbContext database,IMapper mapper)
        {
            _db = database;
            _mapper = mapper;
        }

        private string GenerateBookingID()
        {
            string prefix = "BK";
            var lastBooking = _db.Bookings.OrderByDescending(x => x.BookingID).FirstOrDefault();
            if (lastBooking == null)
            {
                return prefix + "00001";
            }
            else
            {
                var lastNumber = int.Parse(lastBooking.BookingID.Substring(prefix.Length));
                var newNumber = lastNumber + 1;
                return $"{prefix}{newNumber:D5}";
            }
        }
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            var newBoookingID = GenerateBookingID();
            //ViewBag.NewBookingID = newBoookingID;

            var customerList = _db.Customers.Select(c => new SelectListItem
            {
                Value = c.CustomerID.ToString(),
                Text = c.FirstName.ToString() + " " + c.LastName.ToString()
            }).ToList();

            var ViewModel = new AdminBookingViewModel
            {
                BookingID = newBoookingID,
                CustomerList = customerList
            };
            // ViewBag.CustomerList = CustomerList;
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public IActionResult Create(AdminBookingViewModel viewModel)
        {
            if(viewModel.DateGo < viewModel.DateCome)
            {
                ModelState.AddModelError("DateGo", "DateGo must be greater than DateCome");
            }    
            if(string.IsNullOrEmpty(viewModel.BookingID) || viewModel.CustomerID == "--Select CustomerID--" || viewModel.CustomerID == null)
            {
                ModelState.AddModelError("CustomerID", "CustomerID is Required");
            }
            ModelState.Remove("CustomerList");
            if (ModelState.IsValid)
            {
                var existingBooking = _db.Bookings.FirstOrDefault(b => b.BookingID == viewModel.BookingID);
                if (existingBooking == null)
                {
                    var booking = _mapper.Map<Booking>(viewModel);
                    _db.Bookings.Add(booking);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("BookingID", "Booking ID already exists.");
                }
            }
            var CustomerList = _db.Customers.Select(c => new SelectListItem
            {
                Value = c.CustomerID.ToString(),
                Text = c.FirstName.ToString() + " " + c.LastName.ToString()
            }).ToList();
            viewModel.CustomerList = CustomerList;
            viewModel.BookingID = GenerateBookingID();

            //ViewBag.CustomerList = CustomerList;
            
            
            return View(viewModel);
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index(string SortColumn = "BookingID", string iconClass = "fa-sort-asc", int page = 1, string searchQuery = "")
        {
            var booking = _db.Bookings.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                booking = booking.Where(x => x.BookingID.Contains(searchQuery) ||
                                             x.CustomerID.Contains(searchQuery));
            }

            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = iconClass;
            booking = SortBooking(booking, SortColumn, iconClass);

            int NoOfRecoredPerPage = 5;
            int NoOfPages = (int)Math.Ceiling((double)booking.Count() / NoOfRecoredPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecoredPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            booking = booking.Skip(NoOfRecordToSkip).Take(NoOfRecoredPerPage);

            var bookingList = booking.ToList();
            var bookingViewModel = _mapper.Map<IEnumerable<Booking>>(bookingList);

            return View(bookingViewModel);
        }

        private IQueryable<Booking> SortBooking(IQueryable<Booking> booking, string SortColumn, string IconClass)
        {
            switch (SortColumn)
            {
                case "BookingID":
                    booking = IconClass == "fa-sort-asc" ? booking.OrderBy(x => x.BookingID) : booking.OrderByDescending(x => x.BookingID);
                    break;
                case "DateCome":
                    booking = IconClass == "fa-sort-asc" ? booking.OrderBy(x => x.DateCome) : booking.OrderByDescending(x => x.DateCome);
                    break;
                case "DateGo":
                    booking = IconClass == "fa-sort-asc" ? booking.OrderBy(x => x.DateGo) : booking.OrderByDescending(x => x.DateGo);
                    break;
                case "Status":
                    booking = IconClass == "fa-sort-asc" ? booking.OrderBy(x => x.Status) : booking.OrderByDescending(x => x.Status);
                    break;
                case "Deposit":
                    booking = IconClass == "fa-sort-asc" ? booking.OrderBy(x => x.Deposit) : booking.OrderByDescending(x => x.Deposit);
                    break;
                case "NumberPeople":
                    booking = IconClass == "fa-sort-asc" ? booking.OrderBy(x => x.NumberPeople) : booking.OrderByDescending(x => x.NumberPeople);
                    break;
            }
            return booking;
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

            if (id == null || _db.Bookings == null)
            {
                return NotFound();
            }
            var booking = _db.Bookings.Find(id);
            var bookingViewModel = _mapper.Map<AdminBookingViewModel>(booking);
            if (booking == null)
            {
                return NotFound();
            }
            return View(bookingViewModel);
        }
        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, AdminBookingViewModel model)
        {
            if (model.DateGo < model.DateCome)
            {
                ModelState.AddModelError("DateGo", "DateGo must be greater than DateCome");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var booking = _mapper.Map<Booking>(model);
                    _db.Update(booking);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(model.BookingID))
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
            return View(model);
        }

        private bool BookingExists(string id)
        {
            
            return (_db.Bookings?.Any(x => x.BookingID == id)).GetValueOrDefault();
        }
        // Delete
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

            if (id == null || _db.Bookings == null)
            {
                return NotFound();
            }
            var booking = _db.Bookings.Include(b => b.RentForm).FirstOrDefault(x => x.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }
            //if (booking.BookingID.Count() > 0)
            //{
            //    return Content("Can not delete this rooms");
            //}
            
            return View(booking);

        }
        [HttpPost]
        [Route("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            try
            {
                if (_db.Bookings == null)
                {
                    return Problem("Entity set 'Bookings' is null");
                }
                var booking = _db.Bookings?.Find(id);
                if (booking != null)
                {
                    _db.Bookings.Remove(booking);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Can Not Delete this Booking ID ";
                return RedirectToAction("Delete",new {id});
            }
        }
    }
}
