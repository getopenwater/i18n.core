using System.Globalization;

namespace i18n.Core.Abstractions
{
    /// <summary>
    /// Contract to manage the localization.
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        /// Retrieves a dictionary for a specified language tag.
        /// </summary>
        /// <param name="languageTag">The language tag.</param>
        /// <param name="disableCache"></param>
        /// <returns>A <see cref="TranslationDictionary"/> for the specified language tag.</returns>
        TranslationDictionary GetDictionary(string languageTag, bool disableCache = false);

        /// <summary>
        /// Translates text to a given language tag.
        /// </summary>
        /// <param name="languageTag"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        string Translate(string languageTag, string text);
    }
}
