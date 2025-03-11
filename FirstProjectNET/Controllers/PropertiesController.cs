using FirstProjectNET.Data;
using FirstProjectNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstProjectNET.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly HotelDbContext _hotelDbContext;
        public PropertiesController(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }
        public IActionResult Index(int page = 1)
        {
            int NoOfRecordPerPage = 6;
            int totalRoom = _hotelDbContext.Categories.Count();
            int NoOfPages = (int)Math.Ceiling((double)totalRoom / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;

            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;

            var categories = _hotelDbContext.Categories.Include(c => c.Rooms)
                                          .Skip(NoOfRecordToSkip)
                                          .Take(NoOfRecordPerPage)
                                          .ToList();
            ViewBag.Categories = categories;
            return View();
        }
        [HttpGet]
        public IActionResult RoomAvailable(string id, DateTime dateCome, DateTime dateGo, int numPeople)
        {
            if (dateCome > dateGo)
            {
                ModelState.AddModelError("", "Input Wrong Date");
                return View();
            }
            var unavailableRooms = _hotelDbContext.RentForms
                                                .Where(r => (r.DateCheckOut > dateCome) && (dateGo > r.DateCheckIn))
                                                .Select(r => r.RoomID)
                                                .ToList();
            var availableRooms = _hotelDbContext.Rooms
                                                .Where(r => !unavailableRooms.Contains(r.RoomID) && r.Status == "Vacant")
                                                .Include(r => r.Category)
                                                .Include(r => r.Images)
                                                .ToList();
            if (!string.IsNullOrEmpty(id))
            {
                availableRooms = availableRooms.Where(r => r.CategoryID == id).ToList();
            }
            ViewBag.DateCome = dateCome;
            ViewBag.DateGo = dateGo;
            ViewBag.NumberOfPeople = numPeople;
            ViewBag.Rooms = availableRooms;
            return View();
        }
        public IActionResult Details(string ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            var room = _hotelDbContext.Categories.Where(c => c.CategoryID == ID).Include(c => c.Rooms).FirstOrDefault();
            ViewBag.Services = _hotelDbContext.Services.ToList();

            var ImageUrl = _hotelDbContext.Rooms.Join(_hotelDbContext.Images, r => r.RoomID, i => i.RoomID,
                                                        (r, i) => new { r, i })
                                                    .Where(x => x.r.CategoryID == ID).Select(x => x.i.ImageUrl).FirstOrDefault();

            //var RoomService = _hotelDbContext.Rooms
            //                    .Join(_hotelDbContext.RoomServices, r => r.RoomID, rs => rs.RoomID, (r, rs) => new { r, rs })
            //                    .Join(_hotelDbContext.Services, rfs => rfs.rs.ServiceID, s => s.ServiceID, (rrs, s) => new { rrs.r, s })
            //                    .Where(x => x.r.CategoryID == ID)
            //                    .Select(x => x.s.ServiceName)
            //                    .Distinct()
            //                    .ToList();

            ViewBag.ImageUrl = ImageUrl;
            //ViewBag.RoomService = string.Join(", ", RoomService);
            return View(room);
        }
    }
}
