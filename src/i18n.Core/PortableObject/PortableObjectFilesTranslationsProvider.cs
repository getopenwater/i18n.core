using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void LoadTranslations([JetBrains.Annotations.NotNull] CultureInfo cultureInfo, [JetBrains.Annotations.NotNull] CultureDictionary dictionary)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (cultureInfo.Name == "en")
                return;

            foreach (var poFileStream in _poFilesProvider.LoadFiles(cultureInfo.Name))
            {
                using (poFileStream)
                {
                    using var reader = new StreamReader(poFileStream);
                    dictionary.MergeTranslations(_parser.Parse(reader));
                }
            }
        }
    }
}
