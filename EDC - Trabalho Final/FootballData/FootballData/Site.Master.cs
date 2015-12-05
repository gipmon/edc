using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Net;
using Newtonsoft.Json;
using FootballData.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.SqlClient;
using System.Data;
using FootballData.Controllers;


namespace FootballData
{

    public partial class SiteMaster : MasterPage
    {
        private SqlConnection con;
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        public static List<SeasonClass> seasonsList;
        public static string currentUserLanguage;
        public static string menuLanguages;

        protected void Page_Init(object sender, EventArgs e)
        {
            con = ConnectionDB.getConnection();
            // Get Seasons
            String CmdString1 = "SELECT * FROM football.udf_get_season2015_names()";
            SqlCommand cmd1 = new SqlCommand(CmdString1, con);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable("teams");
            sda1.Fill(dt1);

            String html = "";
            int i = 0;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                html += "<li><a href=\"Season.aspx?ID=" + (dt1.Rows[i].ItemArray[1])+"\">"+ dt1.Rows[i].ItemArray[0] + "</a></li>";
            }

            seasons.InnerHtml = html;

            currentUserLanguage = Languages.userLanguage(Request);

            menuLanguages = "";

            foreach (string key in Languages.languages_name.Keys)
            {
                if (key != currentUserLanguage)
                {
                    menuLanguages += "<li><a href=\"ChangeLanguage.aspx?Language=" + key + "\">" + Languages.languages_name[key] + "</a></li>";
                }
            }
            
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected String getUserFullName()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            return currentUser.Name;
        }
    }

}