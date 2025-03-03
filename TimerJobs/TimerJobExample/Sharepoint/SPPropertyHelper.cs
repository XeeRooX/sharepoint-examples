using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerJobExample.Sharepoint
{
    class SPPropertyHelper : IDisposable
    {
        private readonly SPSite _site;

        internal int CleanDay {
            get 
            {
                if (int.TryParse(GetSiteProperty(false, _site, "TimerJobExample__CleanDay"), out int day) && day >= 1 && day <= 28)
                    return day;
                else return 1;
            }
        }

        public SPPropertyHelper(SPSite site)
        {
            _site = site;
        }

        private static string GetSiteProperty(bool isNullValueAllowed, SPSite site, string propertyName)
        {
            string property = null;

            SPSecurity.RunWithElevatedPrivileges(() => {
                property = site.RootWeb.AllProperties[propertyName]?.ToString()?.Trim();
            });

            if (string.IsNullOrEmpty(property) && !isNullValueAllowed)
                throw new Exception($@"На целевом сайте ""{site.Url}"" обязательное WebProperty ""{propertyName}"" отсутствует или не заполнено.");

            return property;
        }

        private static void SetSiteProperty(SPSite site, string propertyName, object value)
        {
            SPSecurity.RunWithElevatedPrivileges(() => {
                site.RootWeb.AllProperties[propertyName] = value;
                site.RootWeb.Update();
            });
        }

        public void Dispose()
        {
            _site.Close();
        }
    }
}
