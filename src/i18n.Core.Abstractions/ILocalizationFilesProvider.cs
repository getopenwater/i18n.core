using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;

namespace i18n.Core.Abstractions
{
    /// <summary>
    /// Contract that provides files containing the localization strings in the file system.
    /// </summary>
    public interface ILocalizationFilesProvider
    {
        IEnumerable<IFileInfo> LoadFiles(string languageTag);
    }
}
