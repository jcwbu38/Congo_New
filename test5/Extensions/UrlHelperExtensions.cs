using Microsoft.AspNetCore.Mvc;
using test5.Controllers;

namespace test5.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(UserController.ConfirmEmail),
                controller: "User",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(UserController.ResetPassword),
                controller: "User",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
