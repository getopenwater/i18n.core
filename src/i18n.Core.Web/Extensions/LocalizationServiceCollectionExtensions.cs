using System;
using System.Diagnostics.CodeAnalysis;
using i18n.Core.Middleware;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public static class LocalizationServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the services to enable localization using Portable Object files.
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static IServiceCollection AddI18NLocalization([JetBrains.Annotations.NotNull] this IServiceCollection services,
            Action<I18NMiddlewareOptions> middlewareOptionsSetupAction = null)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddI18NLocalizationCore();
            services.AddSingleton<IPooledStreamManager>(new DefaultPooledStreamManager());

            services.Configure<I18NMiddlewareOptions>(x =>
            {
                middlewareOptionsSetupAction?.Invoke(x);
            });

            return services;
        }
    }
}
