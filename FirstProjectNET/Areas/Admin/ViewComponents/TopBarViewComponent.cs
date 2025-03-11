using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Areas.Admin.ViewComponents
{
    public class TopBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userName = HttpContext.Session.GetString("Username");            
            return View("RenderTopBar", userName);
        }
    }
}
