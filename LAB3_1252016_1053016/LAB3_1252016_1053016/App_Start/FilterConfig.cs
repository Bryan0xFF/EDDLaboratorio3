using System.Web;
using System.Web.Mvc;

namespace LAB3_1252016_1053016
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
