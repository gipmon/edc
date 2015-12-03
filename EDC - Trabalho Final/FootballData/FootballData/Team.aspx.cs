using FootballData.Controllers;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public TeamClass team;
        public PlayersList players_list;
        public String players_list_html;
        public String fixturesTable_html;
        protected String news_html;
        protected int paginationNews;

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = 1;
            try
            {
                id = int.Parse(Request["ID"]);
            }
            catch (Exception) { }


            var url = "http://api.football-data.org/v1/teams/"+id+"/";
            var syncClient = new WebClient();
            syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
            syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");

            var content = syncClient.DownloadString(url);
            team = JsonConvert.DeserializeObject<TeamClass>(content);
            
            url = team._links.players.href;
            syncClient = new WebClient();
            syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
            syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");

            content = syncClient.DownloadString(url);
            players_list = JsonConvert.DeserializeObject<PlayersList>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            players_list_html += "";

            foreach(Player p in players_list.players)
            {
                players_list_html += "<tr><td>"+p.name+"</td><td>"+p.jerseyNumber+"</td><td>"+p.position+"</td><td>"+p.nationality+"</td><td>"+p.dateOfBirth+"</td><td>"+p.marketValue+"</td><td>"+p.contractUntil+"</td></tr>";
            }


            url = team._links.fixtures.href;
            syncClient = new WebClient();
            syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
            syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");

            content = syncClient.DownloadString(url);
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

            // XML

            url = "https://news.google.pt/news/feeds?pz=1&cf=all&ned=en&hl=pt&q="+ Server.UrlEncode(team .name) + "&output=rss";
            
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
                node_html += "class=\"col-xs-12 col-md-6 col-lg-6\"><div class=\"well\"> <div class=\"media\"> <div class=\"media-body\"> <h4 class=\"media-heading\"><a target=\"_blank\" href=\"" + node.Attributes[2].Value + "\">" + node.Attributes[0].Value + "</a></h4> <p>" + Regex.Replace(Regex.Replace(node.Attributes[1].Value, @"<b><font.*>.*<\/font><\/b><\/font><br>", " "), @"</font><br><font.*><a.*|<b><font.*>.*<\/font><\/b><\/font><br>|<br><font.*>.*</font></a>|​|<nobr>.*<\/nobr>|<.*?>", "")+ "</p><span class=\"text-center\"><small><i class=\"fa fa-calendar - check - o\"></i> " + node.Attributes[5].Value + "</small></span></div></div></div>";
                
                HtmlDocument doc_html = new HtmlDocument();
                doc_html.LoadHtml(node.Attributes[1].Value);

                var html_a = doc_html.DocumentNode.SelectNodes("//a").ToList();

                if (html_a.ToArray().Length > 3)
                {
                    node_html += "<div class=\"btn-group dropup pull-right\" style=\"margin-top: -20px;\"><button type=\"button\" class=\"btn btn-default btn-xs\" disabled=\"disabled\">Related news</button><button type=\"button\" class=\"btn btn-default dropdown-toggle btn-xs\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button><ul class=\"dropdown-menu\">";

                    for (var i=2; i<html_a.ToArray().Length-1; i++)
                    {
                        node_html += "<li><a target=\"_blank\" href=\"" + html_a[i].Attributes[0].Value + "\">" + html_a[i].InnerText + "</a></li>";
                    }

                    node_html += "<!-- Dropdown menu links --></ul></div>";
                }

                news_html += (node_html + "</div>");
            }

            paginationNews = id_int;

            Page.DataBind();
        }
    }
}