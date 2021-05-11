using System;
using System.Diagnostics.CodeAnalysis;
using i18n.Core;
using i18n.Core.Abstractions;
using i18n.Core.Abstractions.Domain;
using i18n.Core.Middleware;
using i18n.Core.PortableObject;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

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
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="hostEnvironment"></param>
        /// <param name="requestLocalizationSetupAction">An action to configure the Microsoft.Extensions.Localization.RequestLocalizationOptions.</param>
        /// <param name="middleWareOptionsSetupAction">An action to configure the i18n.Core.Middleware.I18NMiddlewareOptions</param>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static IServiceCollection AddI18NLocalization([JetBrains.Annotations.NotNull] this IServiceCollection services,
            [JetBrains.Annotations.NotNull] IHostEnvironment hostEnvironment, Action<RequestLocalizationOptions> requestLocalizationSetupAction = null, Action<I18NMiddlewareOptions> middleWareOptionsSetupAction = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (hostEnvironment == null) throw new ArgumentNullException(nameof(hostEnvironment));

            var defaultPooledStreamManager = new DefaultPooledStreamManager();

            services.AddSingleton<INuggetReplacer, DefaultNuggetReplacer>();
            services.AddSingleton<ITranslationProvider, PortableObjectFilesTranslationsProvider>();
            services.AddSingleton<ILocalizationFilesProvider, PortableObjectFilesProvider>();
            services.AddSingleton<ILocalizationManager, LocalizationManager>();
            services.AddSingleton<IPooledStreamManager>(defaultPooledStreamManager);

            services.AddOptions<I18NLocalizationOptions>().BindConfiguration(nameof(I18NLocalizationOptions));

            if (requestLocalizationSetupAction != null)
            {
                services.Configure(requestLocalizationSetupAction);
            }

            if (middleWareOptionsSetupAction != null)
            {
                services.Configure<I18NMiddlewareOptions>(x =>
                {
                    x.CacheEnabled = !hostEnvironment.IsDevelopment();
                    middleWareOptionsSetupAction(x);
                });
            }
            else
            {
                services.AddSingleton(new I18NMiddlewareOptions
                {
                    CacheEnabled = !hostEnvironment.IsDevelopment()
                });
            }

            return services;
        }
    }
}
