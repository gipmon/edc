using FootballData.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FootballData
{
    public partial class ChangeLanguage : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string language = "pt";

                try
                {
                    language = Request["Language"];
                    if (!Languages.domains.ContainsKey(language))
                    {
                        language = "pt";
                    }
                }
                catch (Exception)
                {
                    language = "pt";
                }

                HttpCookie languageCookie = new HttpCookie("UserLanguage");
                languageCookie.Value = language;
                languageCookie.Expires = DateTime.Now.AddDays(15d);
                Response.Cookies.Add(languageCookie);

                Response.Redirect(Request.UrlReferrer.ToString());
            }
            
        }

    }
}