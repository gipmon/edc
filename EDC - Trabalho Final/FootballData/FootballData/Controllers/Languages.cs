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
        public static Hashtable table_name;

        static Languages()
        {
            domains = new Hashtable();
            domains.Add("en", "co.uk");
            domains.Add("de", "de");
            domains.Add("pt", "pt");
            domains.Add("it", "it");
            domains.Add("es", "es");
            domains.Add("fr", "fr");

            table_name = new Hashtable();
            table_name.Add("de", "name");
            table_name.Add("pt", "namePT");
            table_name.Add("en", "nameEN");
            table_name.Add("it", "nameIT");
            table_name.Add("es", "nameES");
            table_name.Add("fr", "nameFR");
            
            languages_name = new Hashtable();
            languages_name.Add("en", "United Kingdom");
            languages_name.Add("de", "Deutschland");
            languages_name.Add("pt", "Portugal");
            languages_name.Add("it", "Italy");
            languages_name.Add("es", "Spain");
            languages_name.Add("fr", "France");
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