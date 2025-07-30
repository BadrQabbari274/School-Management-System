using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            ViewBag.StatusCode = statusCode;

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "عذراً، الصفحة المطلوبة غير موجودة";
                    ViewBag.ErrorDetails = "الرابط الذي تحاول الوصول إليه غير متاح أو تم حذفه";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "خطأ في الخادم";
                    ViewBag.ErrorDetails = "حدث خطأ داخلي في الخادم، يرجى المحاولة لاحقاً";
                    break;
                default:
                    ViewBag.ErrorMessage = "حدث خطأ غير متوقع";
                    ViewBag.ErrorDetails = "يرجى المحاولة مرة أخرى";
                    break;
            }

            return View("Error");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            ViewBag.StatusCode = 500;
            ViewBag.ErrorMessage = "حدث خطأ غير متوقع";
            ViewBag.ErrorDetails = "يرجى المحاولة مرة أخرى";

            return View();
        }
    }
}