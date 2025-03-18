using AutoMapper;
using FirstProjectNET.Data;
using FirstProjectNET.Models;
using FirstProjectNET.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstProjectNET.Controllers
{
    
    public class AccountDetailController : Controller
    {
        private readonly HotelDbContext _db;
        private readonly IMapper _mapper;
        public AccountDetailController(HotelDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult PersonalDetail()
        {
            string id = HttpContext.Session.GetString("AccountID");
            var info = _db.Customers.Include(c => c.Account).FirstOrDefault(c => c.AccountID == int.Parse(id));
            var customerViewModel = _mapper.Map<CustomerViewModel>(info);

            return View(customerViewModel);
        }
        [HttpPost]
        public IActionResult PersonalDetail(CustomerViewModel model)
        {
            ModelState.Remove("Username");
            ModelState.Remove("AccountID");
            ModelState.Remove("CustomerID");
            if(ModelState.IsValid)
            {
                string id = HttpContext.Session.GetString("AccountID");
                var customer = _db.Customers.FirstOrDefault(c => c.AccountID == int.Parse(id));
                if(customer != null)
                {
                    
                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    customer.Email = model.Email;
                    customer.Address = model.Address;
                    customer.Phone = model.Phone;
                    customer.Membership = model.Membership;

                    _db.Update(customer);
                    _db.SaveChanges();

                    var updatedCustomer = _db.Customers.FirstOrDefault(c => c.AccountID == int.Parse(id));
                    var updateModel = _mapper.Map<CustomerViewModel>(updatedCustomer);
                    return View(updateModel);
                }          
            }
            return View(model);
        }

    }
}
