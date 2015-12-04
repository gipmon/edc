using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballData.Controllers
{

    public class Languages
    {
        public static Hashtable domains;
        public static Hashtable languages_name;

        static Languages()
        {
            domains = new Hashtable();
            domains.Add("en", "co.uk");
            domains.Add("pt", "pt");
            domains.Add("de", "de");

            languages_name = new Hashtable();
            languages_name.Add("en", "United Kingdom");
            languages_name.Add("pt", "Portugal");
            languages_name.Add("de", "Deutschland");
        }
        
        public static string userLanguage(HttpRequest Request)
        {
            if (Request.Cookies["UserLanguage"] != null)
            {
                var language = Request.Cookies["UserLanguage"].Value;
                if (!domains.ContainsKey(language))
                {
                    return "pt";
                }
                return language;
            }
            else
            {
                return "pt";
            }
        }
    }
}