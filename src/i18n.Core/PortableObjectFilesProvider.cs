using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using i18n.Core.Abstractions;
using i18n.Core.Abstractions.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace i18n.Core
{
    public class PortableObjectFilesProvider : ILocalizationFilesProvider
    {
        private readonly I18NLocalizationOptions _i18NLocalizationOptions;
        private readonly string _contentRootPath;

        public PortableObjectFilesProvider(IOptions<I18NLocalizationOptions> i18NLocalizationOptions, IHostEnvironment hostEnvironment)
        {
            _i18NLocalizationOptions = i18NLocalizationOptions.Value;
            _contentRootPath = hostEnvironment.ContentRootPath;
        }

        public IEnumerable<Stream> LoadFiles(string languageTag)
        {
            return new[]
            {
                LoadFromEmbeddedResourcesOfWebCommonAssembly(languageTag),
                LoadFromLocaleDirectory(languageTag),
            };
        }

        private Stream LoadFromEmbeddedResourcesOfWebCommonAssembly(string languageTag)
        {
            var assembly = Assembly.LoadFile(BuildAbsolutePath(_i18NLocalizationOptions.CommonLocaleResource));

            return assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Locale.{languageTag}.messages.po");
        }

        private Stream LoadFromLocaleDirectory(string languageTag)
        {
            return File.OpenRead(BuildAbsolutePath(Path.Combine(_i18NLocalizationOptions.LocaleDirectory, languageTag, "messages.po")));
        }

        private string BuildAbsolutePath(string path)
        {
            return Path.IsPathRooted(path) ? path : Path.Combine(_contentRootPath, path);
        }
    }
}
