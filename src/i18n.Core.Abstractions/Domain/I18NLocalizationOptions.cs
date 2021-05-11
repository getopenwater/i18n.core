using System;
using System.Collections.Generic;
using System.Linq;

namespace i18n.Core.Abstractions.Domain
{
    public class I18NLocalizationOptions
    {
        public string CommonLocaleResource { get; set; }
        public string LocaleDirectory { get; set; }
        public string LanguageTagCookieName { get; set; }
    }
}