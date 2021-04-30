using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using i18n.Core.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace i18n.Core
{
    /// <summary>
    /// Represents a manager that manage the localization resources.
    /// </summary>
    public class LocalizationManager : ILocalizationManager
    {
        const string CacheKeyPrefix = "TranslationDictionary-";

        readonly ITranslationProvider _translationProvider;
        readonly IMemoryCache _cache;
        readonly INuggetReplacer _nuggetReplacer;

        /// <summary>
        /// Creates a new instance of <see cref="LocalizationManager"/>.
        /// </summary>
        /// <param name="translationProvider">The <see cref="ITranslationProvider"/>.</param>
        /// <param name="cache">The <see cref="IMemoryCache"/>.</param>
        /// <param name="nuggetReplacer"></param>
        public LocalizationManager(
            ITranslationProvider translationProvider,
            IMemoryCache cache,
            INuggetReplacer nuggetReplacer)
        {
            _translationProvider = translationProvider;
            _cache = cache;
            _nuggetReplacer = nuggetReplacer;
        }

        /// <inheritdocs />
        public TranslationDictionary GetDictionary(string languageTag, bool disableCache = false)
        {
            var cacheKeyPrefix = CacheKeyPrefix + languageTag;

            if (disableCache)
            {
                _cache.Remove(cacheKeyPrefix);
            }

            var cachedDictionary = _cache.GetOrCreate(cacheKeyPrefix, k => new Lazy<TranslationDictionary>(() =>
            {
                var dictionary = new TranslationDictionary(languageTag);

                if (languageTag != "en")
                    _translationProvider.LoadTranslations(languageTag, dictionary);

                return dictionary;
            }, LazyThreadSafetyMode.ExecutionAndPublication));

            return cachedDictionary.Value;
        }

        /// <inheritdocs />
        public string Translate(string languageTag, string text)
        {
            return _nuggetReplacer.Replace(GetDictionary(languageTag), text);
        }
    }
}
