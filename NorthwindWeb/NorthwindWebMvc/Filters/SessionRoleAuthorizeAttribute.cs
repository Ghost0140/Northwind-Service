using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NorthwindWebMvc.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SessionRoleAuthorizeAttribute : ActionFilterAttribute
    {
        public string? RequiredRole { get; set; }
        public string? BlockedRole { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var usuario = session.GetString("usuario");

            if (string.IsNullOrWhiteSpace(usuario))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var rol = session.GetString("rol") ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(RequiredRole) &&
                !string.Equals(rol, RequiredRole, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = GetDefaultRouteByRole(rol);
                return;
            }

            if (!string.IsNullOrWhiteSpace(BlockedRole) &&
                string.Equals(rol, BlockedRole, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = GetDefaultRouteByRole(rol);
            }
        }

        private static RedirectToActionResult GetDefaultRouteByRole(string rol)
        {
            if (string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return new RedirectToActionResult("Index", "Home", null);
            }

            return new RedirectToActionResult("Catalog", "Store", null);
        }
    }
}
