using System;

namespace i18n.Core.Abstractions
{
    /// <summary>
    /// Represents a record in a <see cref="TranslationDictionary"/>.
    /// </summary>
    public class TranslationDictionaryRecord
    {
        /// <summary>
        /// Creates new instance of <see cref="TranslationDictionaryRecord"/>.
        /// </summary>
        /// <param name="messageId">The message Id.</param>
        /// <param name="context">The message context.</param>
        /// <param name="translation">The translation.</param>
        public TranslationDictionaryRecord(string messageId, string context, string translation)
        {
            Key = GetKey(messageId, context);
            Translation = translation;
        }

        /// <summary>
        /// Gets the resource key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the translation.
        /// </summary>
        public string Translation { get; }

        /// <summary>
        /// Retrieved the resource key using <paramref name="messageId"/> and <paramref name="context"/>.
        /// </summary>
        /// <param name="messageId">The message Id.</param>
        /// <param name="context">The message context.</param>
        /// <returns>The resource key.</returns>
        public static string GetKey(string messageId, string context)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentException("MessageId can't be empty.", nameof(messageId));
            }

            if (string.IsNullOrEmpty(context))
            {
                return messageId;
            }

            return context.ToLowerInvariant() + "|" + messageId;
        }
    }
}
