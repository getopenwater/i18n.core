using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using i18n.Core.Abstractions;

namespace i18n.Core.PortableObject
{
    /// <summary>
    /// Represents a provider that provides a translations for .po files.
    /// </summary>
    public class PortableObjectFilesTranslationsProvider : ITranslationProvider
    {
        readonly ILocalizationFilesProvider _poFilesProvider;
        readonly PortableObjectParser _parser;

        /// <summary>
        /// Creates a new instance of <see cref="PortableObjectFilesTranslationsProvider"/>.
        /// </summary>
        /// <param name="poFilesProvider">The <see cref="ILocalizationFilesProvider"/>.</param>
        public PortableObjectFilesTranslationsProvider(ILocalizationFilesProvider poFilesProvider)
        {
            _poFilesProvider = poFilesProvider;
            _parser = new PortableObjectParser();
        }

        /// <inheritdocs />
        public void LoadTranslations([JetBrains.Annotations.NotNull] string languageTag, [JetBrains.Annotations.NotNull] TranslationDictionary dictionary)
        {
            if (languageTag == null)
                throw new ArgumentNullException(nameof(languageTag));

            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            foreach (var fileInfo in _poFilesProvider.LoadFiles(languageTag))
            {
                using var poFileStream = fileInfo.CreateReadStream();
                using var reader = new StreamReader(poFileStream);
                dictionary.MergeTranslations(_parser.Parse(reader));
            }
        }
    }
}
