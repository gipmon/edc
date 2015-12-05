using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FootballData.UserArea
{
    public partial class MyFeed : System.Web.UI.Page
    {
        protected string newsFeed_html;

        protected void Page_Load(object sender, EventArgs e)
        {
            newsFeed_html = "";

            Page.DataBind();
        }
    }
}