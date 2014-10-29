using System;
using System.Collections.Generic;
using ISeeYou.Views;
using ISeeYou.ViewServices;

namespace ISeeYou.Schedulers
{
    public class FetchSettings
    {
        private DateTime _lastFetchSettingsUpdate;
        private readonly TimeSpan _fetchSettingsUpdateInterval = TimeSpan.FromMinutes(1);
        private PhotoFetchSettings _fetchSettings;

        private readonly SitesViewService _siteService;

        public FetchSettings(SitesViewService siteService)
        {
            _siteService = siteService;
        }

        public PhotoFetchSettings GetFetchSettings()
        {
            if (DateTime.UtcNow < _lastFetchSettingsUpdate + _fetchSettingsUpdateInterval)
                return _fetchSettings;

            _lastFetchSettingsUpdate = DateTime.UtcNow;

            var siteSettings = _siteService.GetSite();
            if (siteSettings == null)
            {
                siteSettings = new SiteView
                {
                    Id = SiteSettings.SiteId,
                    PhotoFetchSettings = DefaultFetchSettings()
                };
                _siteService.InsertAsync(siteSettings);
            }

            if (siteSettings.PhotoFetchSettings == null)
            {
                siteSettings.PhotoFetchSettings = DefaultFetchSettings();
                _siteService.Save(siteSettings);
            }

            return _fetchSettings = siteSettings.PhotoFetchSettings;
        }

        private static PhotoFetchSettings DefaultFetchSettings()
        {
            return new PhotoFetchSettings
            {
                DelayBase = 50,
                Disabled = false,
                Categories = new List<PhotoCategory>
                {
                    new PhotoCategory {Age = 2, Ratio = 5},
                    new PhotoCategory {Age = 4, Ratio = 6},
                    new PhotoCategory {Age = 10, Ratio = 10},
                    new PhotoCategory {Age = 30, Ratio = 15},
                    new PhotoCategory {Age = int.MaxValue, Ratio = 20},
                }
            };
        }

        public bool IsAppDisabled()
        {
            return GetFetchSettings().Disabled;
        }
    }
}