using System.Collections.Generic;
using System.IO;

namespace i18n.Core.Abstractions
{
    /// <summary>
    /// Contract that provides files containing the localization strings in the file system.
    /// </summary>
    public interface ILocalizationFilesProvider
    {
        IEnumerable<Stream> LoadFiles(string languageTag);
    }
}
