using System.Web;
using System.Web.Mvc;

namespace Kirgaz_Online_Islemleri_Portal_Son_kopya
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
