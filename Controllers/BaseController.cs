using Microsoft.AspNetCore.Mvc;

namespace NTTCinemas.Controllers
{
    public class BaseController : Controller
    {
        protected void SetAlertNotice(string message, string type)
        {
            TempData["NoticeMessage"] = message;

            if (type == "success")
                TempData["NoticeType"] = "alert-success";
            else if (type == "warning")
                TempData["NoticeType"] = "alert-warning";
            else if (type == "info")
                TempData["NoticeType"] = "alert-info";
        }
    }
}
