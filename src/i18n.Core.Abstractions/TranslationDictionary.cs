using System;
using System.Collections.Generic;

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
        /// <param name="pluralRule">The pluralization rule.</param>
        public TranslationDictionary(string languageTag, PluralizationRuleDelegate pluralRule)
        {
            Translations = new Dictionary<string, string[]>();
            LanguageTag = languageTag;
            PluralRule = pluralRule;
        }

        /// <summary>
        /// Gets the language tag.
        /// </summary>
        public string LanguageTag { get; }

        /// <summary>
        /// gets the pluralization rule.
        /// </summary>
        public PluralizationRuleDelegate PluralRule { get; }

        /// <summary>
        /// Gets the localized value.
        /// </summary>
        /// <param name="messageId">The message ID.</param>
        /// <param name="context">The message context (comment).</param>
        /// <returns></returns>
        public string this[string messageId, string context] => this[TranslationDictionaryRecord.GetKey(messageId, context), (int?)null];

        /// <summary>
        /// Gets the localized value.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <param name="count">The number to specify the pluralization form.</param>
        /// <returns></returns>
        public string this[string key, int? count]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (!Translations.TryGetValue(key, out var translations))
                {
                    return null;
                }

                var pluralForm = count.HasValue ? PluralRule(count.Value) : 0;
                if (pluralForm >= translations.Length)
                {
                    throw new PluralFormNotFoundException($"Plural form '{pluralForm}' doesn't exist for the key '{key}' in the '{LanguageTag}' language tag.");
                }

                return translations[pluralForm];
            }
        }

        /// <summary>
        /// Gets a list of the translations including the plural forms.
        /// </summary>
        public IDictionary<string, string[]> Translations { get; }

        /// <summary>
        /// Merges the translations from multiple dictionary records.
        /// </summary>
        /// <param name="records">The records to be merged.</param>
        public void MergeTranslations(IEnumerable<TranslationDictionaryRecord> records)
        {
            foreach (var record in records)
            {
                Translations[record.Key] = record.Translations;
            }
        }
    }
}
