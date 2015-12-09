using FootballData.Controllers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace FootballData.UserArea
{
    
    public partial class MyFeed : System.Web.UI.Page
    {
        protected string newsFeed_html;
        private SqlConnection con;
        protected LinkedList<TeamSql> teamsList;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                con = ConnectionDB.getConnection();

                newsFeed_html = "";

                String CmdString = "SELECT * FROM football.udf_get_teams_subscribed(@userId)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@userId", System.Web.HttpContext.Current.User.Identity.GetUserId());
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("teams");
                sda.Fill(dt);

                teamsList = new LinkedList<TeamSql>();
                var feed_language = Languages.userLanguage(Request);

                var columnName = "name" + feed_language.ToUpper();
                var index = dt.Columns.IndexOf(columnName);
                foreach (DataRow row in dt.Rows)
                {
                    TeamSql tmp = new TeamSql(row, index);
                    teamsList.AddLast(tmp);
                }

                teams.DataSource = teamsList;
                teams.DataValueField = "id";
                teams.DataTextField = "name";

                Page.DataBind();

                foreach (ListItem itm in teams.Items)
                {
                    itm.Selected = true;
                }

                setNews();
            }
        }
        
        protected void teams_SelectedIndexChanged(object sender, EventArgs e)
        {
            setNews();
        }

        protected void typeOfRss_SelectedIndexChanged(object sender, EventArgs e)
        {
            setNews();
        }

        protected void setNews()
        {
            var url = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var teamListStr = "";

            var format = "simple";

            foreach (ListItem itm in typeOfRss.Items)
            {
                if (itm.Selected)
                {
                    format = itm.Value;
                }
            }


            foreach (ListItem itm in teams.Items)
            {
                if (itm.Selected)
                {
                    teamListStr = teamListStr + itm.Value + ",";
                }
            }

            if (teamListStr.Length == 0)
            {
                teamListStr = "";
            }
            else
            {
                teamListStr = teamListStr.Remove(teamListStr.Length - 1, 1);
            }
            
            var language = Languages.userLanguage(Request);

            url = url + "/rss.aspx?language=" + language + "&format=" + format + "&teamList=" + teamListStr + "&userId=" + userId;

            rssFeedLink.Text = url;
            urlBtn.PostBackUrl = url;

            XmlReader reader = XmlReader.Create(url);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();

            XmlDataSource_feed.Data = doc.OuterXml;
            XmlDataSource_feed.DataBind();
            XmlDataSource_feed.XPath = "/rss/channel";

            XmlDocument xdoc = XmlDataSource_feed.GetXmlDocument();
            XmlElement root = xdoc.DocumentElement;

            XmlNodeList nodes_items = root.SelectNodes("/rss/channel/item");

            LinkedList<TeamNewRss> tn = new LinkedList<TeamNewRss>();
            LinkedList<TeamNewRss> tn1 = new LinkedList<TeamNewRss>();
            LinkedList<TeamNewRss> tn2 = new LinkedList<TeamNewRss>();

            TeamNewRss.idCount = 0;

            foreach (XmlNode node in nodes_items)
            {
                if (TeamNewRss.idCount >= 20)
                {
                    tn2.AddLast(new TeamNewRss(node));
                }
                else if (TeamNewRss.idCount >= 10)
                {
                    tn1.AddLast(new TeamNewRss(node));
                }
                else
                {
                    tn.AddLast(new TeamNewRss(node));
                }
            }
            rssHtmlTab2.DataSource = tn2;
            rssHtmlTab2.DataBind();
            rssHtmlTab1.DataSource = tn1;
            rssHtmlTab1.DataBind();
            rssHtmlTab.DataSource = tn;
            rssHtmlTab.DataBind();
        }

        protected class TeamNewRss
        {
            public static int idCount = 0;

            public string title { get; set; }
            public string link { get; set; }
            public string team { get; set; }
            public string teamId { get; set; }
            public string description { get; set; }
            public int id { get; set; }

            public TeamNewRss(XmlNode xn)
            {
                this.title = xn.Attributes[0].Value;
                this.link = xn.Attributes[1].Value;
                this.team = xn.Attributes[2].Value;
                this.teamId = xn.Attributes[3].Value;
                this.description = xn.Attributes[4].Value;
                this.id = idCount++;
            }
            
            public static string truncate(string value, int maxLength)
            {
                if (string.IsNullOrEmpty(value)) return value;
                return value.Length <= maxLength ? value : value.Substring(0, maxLength);
            }
        }
        
        protected class TeamSql
        {
            public string name { get; set; }
            public int id { get; set; }

            public TeamSql(DataRow dt, int index)
            {
                this.id = (int)dt.ItemArray[0];
                this.name = (string)dt.ItemArray[index];
            }
        }

    }
}