using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PRN222
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        // Định nghĩa role cần thiết (ví dụ "1" cho staff, "2" cho lecture)
        public string RequiredRole { get; set; } = string.Empty;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy role từ session
            var role = context.HttpContext.Session.GetString("AccountRole");

            // Nếu không có session hoặc role không trùng khớp, chuyển hướng về trang đăng nhập
            if (string.IsNullOrEmpty(role) || role != RequiredRole)
            {
                context.Result = new RedirectToActionResult("Login", "SystemAccount", null);
            }

            base.OnActionExecuting(context);
        }
    }   
}
