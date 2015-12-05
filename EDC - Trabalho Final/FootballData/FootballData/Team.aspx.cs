using FootballData.Controllers;
using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace FootballData
{
    public partial class Team : System.Web.UI.Page
    {

        public string teamName;
        public TeamClass team;
        public string teamSquadValue;
        public string teamCrestURL;
        public PlayersList players_list;
        public String players_list_html;
        public String fixturesTable_html;
        public String leaguesHistory_html;
        public int id;
        protected String news_html;
        protected int paginationNews;

        protected string subscribe_html;

        private SqlConnection con;

        protected int db_news = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            con = ConnectionDB.getConnection();

            var feed_language = Languages.userLanguage(Request);

            id = 1;
            try
            {
                id = int.Parse(Request["ID"]);
            }
            catch (Exception) { }

            // Get Team
            String CmdString4 = "SELECT * FROM football.udf_get_team(@teamID)";
            SqlCommand cmd4 = new SqlCommand(CmdString4, con);
            cmd4.Parameters.AddWithValue("@teamID", id);
            SqlDataAdapter sda4 = new SqlDataAdapter(cmd4);
            DataTable dt4 = new DataTable("season");
            sda4.Fill(dt4);

            teamName = dt4.Rows[0].ItemArray[1].ToString();
            teamCrestURL = dt4.Rows[0].ItemArray[3].ToString();
            teamSquadValue = dt4.Rows[0].ItemArray[2].ToString();


            // subscribe btns
            if((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // see if user is subscribing the team or not
                string cmd_str = "SELECT football.udf_user_subscribed_team(@user_id, @team_id)";
                SqlCommand cmd_subscribe = new SqlCommand(cmd_str, con);
                cmd_subscribe.Parameters.AddWithValue("@user_id", System.Web.HttpContext.Current.User.Identity.GetUserId());
                cmd_subscribe.Parameters.AddWithValue("@team_id", Convert.ToInt32(id));
                cmd_subscribe.CommandType = CommandType.Text;

                int subscribing = 0;

                try
                {
                    con.Open();
                    subscribing = (int)cmd_subscribe.ExecuteScalar();
                    con.Close();
                }
                catch (Exception exc)
                {
                    con.Close();
                }

                if (subscribing == 0)
                {
                    subscribe_html = "<a class=\"btn icon-btn btn-success\" href=\"UserArea/SubscribeTeam.aspx?TeamID=" + id + "\"><span class=\"glyphicon btn-glyphicon glyphicon-plus img-circle text-success\"></span> Subscribe</a>";
                }
                else
                {
                    subscribe_html = "<a class=\"btn icon-btn btn-warning\" href=\"UserArea/SubscribeTeam.aspx?TeamID=" + id + "\"><span class=\"glyphicon btn-glyphicon glyphicon-minus img-circle text-warning\"></span> Unsubscribe</a>";
                }
            }
            else
            {
                subscribe_html = "<a class=\"btn icon-btn btn-success\" href=\"Account/Login\"><span class=\"glyphicon btn-glyphicon glyphicon-plus img-circle text-success\"></span> Subscribe</a>";
            }

            String CmdString3 = "SELECT * FROM football.udf_get_players(@teamID)";
            SqlCommand cmd3 = new SqlCommand(CmdString3, con);
            cmd3.Parameters.AddWithValue("@teamID", id);
            SqlDataAdapter sda3 = new SqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable("players");
            sda3.Fill(dt3);
            players_list_html += "";

            for(var i = 0; i<dt3.Rows.Count; i++)
            {
                players_list_html += "<tr><td>" + dt3.Rows[i].ItemArray[1] + "</td><td>" + dt3.Rows[i].ItemArray[6] + "</td><td>" + dt3.Rows[i].ItemArray[5] + "</td><td>" + dt3.Rows[i].ItemArray[3] + "</td><td>" + dt3.Rows[i].ItemArray[2] + "</td><td>" + dt3.Rows[i].ItemArray[8] + "</td><td>" + dt3.Rows[i].ItemArray[7] + "</td></tr>";
            }
            
            string url = dt4.Rows[0].ItemArray[4].ToString();
            WebClient syncClient = new WebClient();
            syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
            syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");

            var content = syncClient.DownloadString(url);
            var team_fixtures_list = JsonConvert.DeserializeObject<FixtureTeam>(content);

            foreach (FixtureGame fix in team_fixtures_list.fixtures)
            {
                fixturesTable_html += "<tr><td>" + fix.date.ToString().Replace("Z", " ").Replace("T", " ") + "</td><td><a href=\"Team.aspx?ID=" + fix._links.homeTeam.href.ToString().Replace("http://api.football-data.org/v1/teams/", "") + "\">" + fix.homeTeamName + "</a></td><td><a href=\"Team.aspx?ID=" + fix._links.awayTeam.href.ToString().Replace("http://api.football-data.org/v1/teams/", "") + "\">" + fix.awayTeamName + "</a></td>";
                if (fix.status == "FINISHED")
                {
                    fixturesTable_html += "<td>" + fix.result.goalsHomeTeam + "-" + fix.result.goalsAwayTeam + "</td>";
                }
                else
                {
                    fixturesTable_html += "<td>--</td>";
                }
                fixturesTable_html += "</tr>";
            }

            String CmdString5 = "SELECT * FROM football.udf_get_leagues(@teamID)";
            SqlCommand cmd5 = new SqlCommand(CmdString5, con);
            cmd5.Parameters.AddWithValue("@teamID", id);
            SqlDataAdapter sda5 = new SqlDataAdapter(cmd5);
            DataTable dt5 = new DataTable("leagues");
            sda5.Fill(dt5);
            leaguesHistory_html += "";

            for (var i = 0; i < dt5.Rows.Count; i++)
            {
                leaguesHistory_html += "<tr><td><a href=\"Season.aspx?ID=" + (dt5.Rows[i].ItemArray[0]) + "\">" + dt5.Rows[i].ItemArray[1] + "</a></td><td>" + dt5.Rows[i].ItemArray[2] + "</td></tr>";
            }

            // NEWWWWS
            // see if team has news stored and subscribers
            string CmdString = "SELECT football.udf_team_has_news(@team_id, @language)";
            SqlCommand cmd = new SqlCommand(CmdString, con);
            cmd.Parameters.AddWithValue("@team_id", Convert.ToInt32(id));
            cmd.Parameters.AddWithValue("@language", feed_language);
            cmd.CommandType = CommandType.Text;

            string CmdString6 = "SELECT football.udf_team_has_news(@team_id, @language)";
            SqlCommand cmd6 = new SqlCommand(CmdString6, con);
            cmd6.Parameters.AddWithValue("@team_id", Convert.ToInt32(id));
            cmd6.CommandType = CommandType.Text;

            try
            {
                con.Open();
                db_news = (int)cmd.ExecuteScalar();
                db_news += (int)cmd6.ExecuteScalar();
                con.Close();
            }
            catch (Exception)
            {
                con.Close();
            }

            if (db_news == 0)
            {
                // google find

                Hashtable domains = new Hashtable();
                domains.Add("en", "co.uk");
                domains.Add("pt", "pt");
                domains.Add("de", "de");

                url = "https://news.google."+ Languages.domains[feed_language] + "/news/feeds?pz=1&cf=all&q=" + Server.UrlEncode(teamName) + "&output=rss";

                XmlReader reader = XmlReader.Create(url);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                XmlDataSourceGoogle_feed.Data = doc.OuterXml;
                XmlDataSourceGoogle_feed.DataBind();
                XmlDataSourceGoogle_feed.XPath = "/rss/channel";

                XmlDocument xdoc = XmlDataSourceGoogle_feed.GetXmlDocument();
                XmlElement root = xdoc.DocumentElement;
                XmlNodeList nodes_items = root.SelectNodes("/rss/channel/item");

                news_html = "";

                int id_int = 0;

                foreach (XmlNode node in nodes_items)
                {
                    id_int++;

                    String node_html = "<div id=\"new_id_" + id_int + "\"";
                    node_html += "class=\"col-xs-12 col-md-6 col-lg-6\"><div class=\"well\"> <div class=\"media\"> <div class=\"media-body\"> <h4 class=\"media-heading\"><a target=\"_blank\" href=\"" + node.Attributes[2].Value + "\">" + node.Attributes[0].Value + "</a></h4> <p>" + Regex.Replace(Regex.Replace(node.Attributes[1].Value, @"<b><font.*>.*<\/font><\/b><\/font><br>", " "), @"</font><br><font.*><a.*|<b><font.*>.*<\/font><\/b><\/font><br>|<br><font.*>.*</font></a>|​|<nobr>.*<\/nobr>|<.*?>", "") + "</p><span class=\"text-center\"><small><i class=\"fa fa-calendar - check - o\"></i> " + node.Attributes[5].Value + "</small></span></div></div></div>";

                    HtmlDocument doc_html = new HtmlDocument();
                    doc_html.LoadHtml(node.Attributes[1].Value);

                    var html_a = doc_html.DocumentNode.SelectNodes("//a").ToList();

                    if (html_a.ToArray().Length > 3)
                    {
                        node_html += "<div class=\"btn-group dropup pull-right\" style=\"margin-top: -20px;\"><button type=\"button\" class=\"btn btn-default btn-xs\" disabled=\"disabled\">Related news</button><button type=\"button\" class=\"btn btn-default dropdown-toggle btn-xs\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button><ul class=\"dropdown-menu\">";

                        for (var i = 2; i < html_a.ToArray().Length - 1; i++)
                        {
                            node_html += "<li><a target=\"_blank\" href=\"" + html_a[i].Attributes[0].Value + "\">" + html_a[i].InnerText + "</a></li>";
                        }

                        node_html += "<!-- Dropdown menu links --></ul></div>";
                    }

                    news_html += (node_html + "</div>");
                }

                paginationNews = id_int;
            }
            else
            {
                String CmdString1 = "SELECT * FROM football.udf_get_team_news(@team_id, @language)";
                SqlCommand cmd1 = new SqlCommand(CmdString1, con);
                cmd1.Parameters.AddWithValue("@team_id", Convert.ToInt32(id));
                cmd1.Parameters.AddWithValue("@language", feed_language);
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable("teams");
                sda1.Fill(dt1);

                int id_int = 0;
                news_html = "";
                
                foreach (DataRow teamNew in dt1.Rows)
                {
                    id_int++;

                    var news_id = (int)teamNew.ItemArray[0];
                    var news_title = (string)teamNew.ItemArray[1];
                    var news_link = (string)teamNew.ItemArray[2];
                    var news_description = (string)teamNew[3];
                    var news_date = (DateTime)teamNew[4];

                    var node_html = (string)"<div id=\"new_id_" + id_int + "\"";
                    node_html += "class=\"col-xs-12 col-md-6 col-lg-6\"><div class=\"well\"> <div class=\"media\"> <div class=\"media-body\"> <h4 class=\"media-heading\"><a target=\"_blank\" href=\"" + news_link + "\">" + news_title + "</a></h4> <p>" + news_description + "</p><span class=\"text-center\"><small><i class=\"fa fa-calendar - check - o\"></i> " + news_date.ToString() + "</small></span></div></div></div>";

                    // related news
                    String CmdString2 = "SELECT * FROM football.udf_get_team_news_related(@related_id)";
                    SqlCommand cmd2 = new SqlCommand(CmdString2, con);
                    cmd2.Parameters.AddWithValue("@related_id", Convert.ToInt32(news_id));
                    SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable("relatedNews");
                    sda2.Fill(dt2);

                    if (dt2.Rows.Count != 0)
                    {
                        node_html += "<div class=\"btn-group dropup pull-right\" style=\"margin-top: -20px;\"><button type=\"button\" class=\"btn btn-default btn-xs\" disabled=\"disabled\">Related news</button><button type=\"button\" class=\"btn btn-default dropdown-toggle btn-xs\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button><ul class=\"dropdown-menu\">";

                        foreach (DataRow teamRelatedNews in dt2.Rows)
                        {
                            var related_id = (int)teamRelatedNews.ItemArray[0];
                            var title = (string)teamRelatedNews.ItemArray[1];
                            var link = (string)teamRelatedNews.ItemArray[2];

                            node_html += "<li><a target=\"_blank\" href=\"" + link + "\">" + title + "</a></li>";
                        }

                        node_html += "<!-- Dropdown menu links --></ul></div>";
                    }
                    
                    news_html += (node_html + "</div>");
                }
                
                paginationNews = id_int;
            }
            
            Page.DataBind();
        }
    }
}