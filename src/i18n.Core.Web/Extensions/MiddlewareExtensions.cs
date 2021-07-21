using System.Diagnostics.CodeAnalysis;
using i18n.Core.Middleware;
using Microsoft.AspNetCore.Builder;

namespace i18n.Core.Extensions
{
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseI18NRequestLocalization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<I18NMiddleware>();
        }
    }
}
