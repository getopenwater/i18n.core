using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace i18n.Core.Abstractions.Domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class I18NLocalizationOptions
    {
        private const string CommonLocaleResourceDefault = "./bin/Debug/net5.0/AwardsCms.Web.Common.Core.dll";
        private const string LocaleDirectoryDefault = "Locale";

        private readonly ISettingsProvider _settingsProvider;

        public string CommonLocaleResource
        {
            get
            {
                var path = _settingsProvider.GetSetting(GetPrefixedString("CommonLocaleResource")) ?? CommonLocaleResourceDefault;

                return BuildAbsolutePathFromProjectDirectory(path);
            }
            set => _settingsProvider.SetSetting(GetPrefixedString("CommonLocaleResource"), value);
        }

        public string LocaleDirectory
        {
            get
            {
                var path = _settingsProvider.GetSetting(GetPrefixedString("LocaleDirectory")) ?? LocaleDirectoryDefault;

                return BuildAbsolutePathFromProjectDirectory(path);
            }
            set => _settingsProvider.SetSetting(GetPrefixedString("LocaleDirectory"), value);
        }


        public I18NLocalizationOptions() : this(new SettingsProvider(Directory.GetCurrentDirectory()))
        {
        }

        public I18NLocalizationOptions(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }

        private static string GetPrefixedString(string key)
        {
            return "i18n." + key;
        }

        private string BuildAbsolutePathFromProjectDirectory(string path)
        {
            return Path.IsPathRooted(path) ? path : Path.GetFullPath(Path.Combine(_settingsProvider.ProjectDirectory ?? throw new InvalidOperationException(), path));
        }
    }
}