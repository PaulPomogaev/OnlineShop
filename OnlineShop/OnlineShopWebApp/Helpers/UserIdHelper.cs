using System.Text;

namespace OnlineShopWebApp.Helpers
{
    public static class UserIdHelper
    {
        public static string GetUserId(HttpContext httpContext)
        {
            var user = httpContext.User;
            if (user?.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(user.Identity.Name))
            {
                return user.Identity.Name;
            }

            TryInitializeSession(httpContext);

            var sessionId = httpContext.Session.Id;
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString("N");
            }

            return $"guest_{sessionId}";
        }

        private static void TryInitializeSession(HttpContext httpContext)
        {
            try
            {
                if (string.IsNullOrEmpty(httpContext.Session.GetString("__session_init")))
                {
                    httpContext.Session.SetString("__session_init", DateTime.Now.ToString("O"));
                    var id = httpContext.Session.Id;
                }
            }
            catch
            {
                
            }
        }
    }
}
