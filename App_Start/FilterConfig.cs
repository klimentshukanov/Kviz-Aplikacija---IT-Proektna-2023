using System.Web;
using System.Web.Mvc;

namespace Kviz_Aplikacija___IT_Proektna_2023
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
