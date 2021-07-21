using System;
using System.Diagnostics.CodeAnalysis;
using i18n.Core;
using i18n.Core.Abstractions;
using i18n.Core.PortableObject;

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
        public static IServiceCollection AddI18NLocalizationCore([JetBrains.Annotations.NotNull] this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddMemoryCache();
            services.AddSingleton<INuggetReplacer, DefaultNuggetReplacer>();
            services.AddSingleton<ITranslationProvider, PortableObjectFilesTranslationsProvider>();
            services.AddSingleton<ILocalizationManager, LocalizationManager>();

            return services;
        }
    }
}
