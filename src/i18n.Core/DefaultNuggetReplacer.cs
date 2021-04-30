using System;
using System.Text;
using System.Text.RegularExpressions;
using i18n.Core.Abstractions;
using JetBrains.Annotations;

namespace i18n.Core
{
    public interface INuggetReplacer
    {
        string Replace([NotNull] TranslationDictionary translationDictionary, string text);
    }

    public class DefaultNuggetReplacer : INuggetReplacer
    {
        private const string NuggetBeginToken = "[[[";
        private const string NuggetEndToken = "]]]";
        private const string NuggetDelimiterToken = "|||";
        private const string NuggetCommentToken = "///";

        private static readonly Regex NuggetRegex;

        // https://github.com/turquoiseowl/i18n/blob/ce7bdc9d8a8b92022c42417edeff4fb9ce8d3170/src/i18n.Domain/Helpers/NuggetParser.cs#L149

        static DefaultNuggetReplacer()
        {
            // Prep the regexes. We escape each token char to ensure it is not misinterpreted.
            // · Breakdown e.g. "\[\[\[(.+?)(?:\|\|\|(.+?))*(?:\/\/\/(.+?))?\]\]\]"
            NuggetRegex = new Regex(
                string.Format(@"{0}(.+?)(?:{1}(.*?))*(?:{2}(.+?))?{3}",
                    Regex.Escape(NuggetBeginToken),
                    Regex.Escape(NuggetDelimiterToken),
                    Regex.Escape(NuggetCommentToken),
                    Regex.Escape(NuggetEndToken)),
                    RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled);
        }

        public string Replace(TranslationDictionary translationDictionary, string text)
        {
            if (translationDictionary == null)
                throw new ArgumentNullException(nameof(translationDictionary));

            return NuggetRegex.Replace(text, match =>
            {
                var messageId = match.Groups[1].Value;
                var context = match.Groups[3].Value;
                var translatedText = translationDictionary[messageId, context] ?? messageId;

                var formatItemCaptures = match.Groups[2].Captures;
                for (var i = 0; i < formatItemCaptures.Count; i++)
                {
                    translatedText = translatedText.Replace($"%{i}", formatItemCaptures[i].Value);
                }

                return translatedText;
            });
        }
    }
}
