using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using ReactiveUI;
using uWidgets.Core.Interfaces;
using uWidgets.Locales;

namespace uWidgets.ViewModels;

public class LanguageViewModel(IAppSettingsProvider appSettingsProvider) : ReactiveObject
{
    public CultureInfo[] Languages => GetAvailableCultures().OrderBy(x => x.DisplayName).ToArray();
    
    public CultureInfo Language
    {
        get => CultureInfo.GetCultureInfo(appSettingsProvider.Get().Region.Language);
        set
        {
            var settings = appSettingsProvider.Get();
            var newRegion = settings.Region with { Language = value.Name };
            var newSettings = settings with { Region = newRegion };
            appSettingsProvider.Save(newSettings);
        }
    }
    
    private static IEnumerable<CultureInfo> GetAvailableCultures()
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

        yield return new CultureInfo("en");
        
        foreach (var culture in cultures)
        {
            if (culture.Equals(CultureInfo.InvariantCulture)) continue;
            ResourceSet? resourceSet = null;
            
            try
            {
                resourceSet = Locale.ResourceManager.GetResourceSet(culture, true, false);
            }
            catch (CultureNotFoundException)
            {
                
            }
            
            if (resourceSet != null)
                yield return culture;
        }
    }
}