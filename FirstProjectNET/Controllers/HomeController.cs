using AutoMapper;
using FirstProjectNET.Data;
using FirstProjectNET.Models;
using FirstProjectNET.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.WebSockets;

namespace FirstProjectNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelDbContext _hotelDbContext;
        private readonly IMapper _iMapper;

        public HomeController(ILogger<HomeController> logger,HotelDbContext hotelDbContext,IMapper Mapper)
        {
            _logger = logger;
            _hotelDbContext = hotelDbContext;
            _iMapper = Mapper;
        }

        public IActionResult Index()
        {
            var previewRoom = _hotelDbContext.Categories.OrderBy(s => s.Price).ToList();
            var rooms = new List<Category>();
            Dictionary<string,bool> map = new Dictionary<string, bool>();

            foreach (var room in previewRoom)
            {
                string currId = room.CategoryID.Substring(0,room.CategoryID.Length -2);
                map[currId] = true;
            }
            foreach (var room in previewRoom)
            {
                string currID = room.CategoryID.Substring(0, room.CategoryID.Length -2);
                if (map[currID] == true)
                {
                    rooms.Add(room);
                    map[currID] = false;
                }
            }

            var roomCategory = _iMapper.Map<IEnumerable<HomeViewModel.CategoryViewModel>>(rooms);


            var Services = _hotelDbContext.Services.Where(s => s.ServiceID == "S0006" || s.ServiceID == "S0007" || s.ServiceID == "S0008").ToList();
            var serviceCategory = _iMapper.Map<IEnumerable<HomeViewModel.ServiceViewModel>>(Services);

            var rates = _hotelDbContext.Rates.ToList();
            var rateCategory = _iMapper.Map<IEnumerable<HomeViewModel.RateViewModel>>(rates);

            var model = new HomeViewModel
            {
                Services = serviceCategory,
                Rates = rateCategory,
                Categories = roomCategory
            };

            //ViewBag.Rooms = rooms;
            return View(model);
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
    }
}
