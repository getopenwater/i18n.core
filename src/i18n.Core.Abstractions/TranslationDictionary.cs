using System;
using System.Collections.Generic;
using System.Linq;

namespace i18n.Core.Abstractions
{
    /// <summary>
    /// Represents a dictionary for a certain language tag.
    /// </summary>
    public class TranslationDictionary
    {
        /// <summary>
        /// Creates a new instance of <see cref="TranslationDictionary"/>.
        /// </summary>
        /// <param name="languageTag">The language tag.</param>
        public TranslationDictionary(string languageTag)
        {
            Translations = new Dictionary<string, string>();
            LanguageTag = languageTag;
        }

        /// <summary>
        /// Gets the language tag.
        /// </summary>
        public string LanguageTag { get; }

        /// <summary>
        /// Gets the localized value.
        /// </summary>
        /// <param name="messageId">The message ID.</param>
        /// <param name="context">The message context (comment).</param>
        /// <returns></returns>
        public string this[string messageId, string context]
        {
            get
            {
                var key = TranslationDictionaryRecord.GetKey(messageId, context);

                return Translations.TryGetValue(key, out var translation) ? translation : null;
            }
        }

        /// <summary>
        /// Gets a list of the translations including the plural forms.
        /// </summary>
        public IDictionary<string, string> Translations { get; }

        /// <summary>
        /// Merges the translations from multiple dictionary records.
        /// </summary>
        /// <param name="records">The records to be merged.</param>
        public void MergeTranslations(IEnumerable<TranslationDictionaryRecord> records)
        {
            foreach (var record in records)
            {
                Translations[record.Key] = record.Translation;
            }
        }
    }
}
