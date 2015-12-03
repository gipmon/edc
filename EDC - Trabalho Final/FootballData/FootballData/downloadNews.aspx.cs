using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace FootballData
{
    public partial class downloadNews : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var team_id = 503;
            var team_name = "FC Porto";

            var url = "https://news.google.pt/news/feeds?pz=1&cf=all&ned=en&hl=pt&q=" + Server.UrlEncode(team_name) + "&output=rss";

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
            
            foreach (XmlNode node in nodes_items)
            {
                var news_url = "";

                if (node.Attributes[3].Value == "false")
                {
                    var urls = node.Attributes[2].Value.ToString().Split(new string[] { "&url=" }, StringSplitOptions.None);

                    if (urls.Length == 2)
                    {
                        news_url = urls[1];
                    }
                    else
                    {
                        news_url = node.Attributes[2].Value;
                    }
                }
                else
                {
                    news_url = node.Attributes[2].Value;
                }
                
                var news_title = node.Attributes[0].Value;
                var news_description = Regex.Replace(Regex.Replace(node.Attributes[1].Value, @"<b><font.*>.*<\/font><\/b><\/font><br>", " "), @"</font><br><font.*><a.*|<b><font.*>.*<\/font><\/b><\/font><br>|<br><font.*>.*</font></a>|​|<nobr>.*<\/nobr>|<.*?>", "");
                var news_pubDate = node.Attributes[5].Value;
                
                HtmlDocument doc_html = new HtmlDocument();
                doc_html.LoadHtml(node.Attributes[1].Value);

                var html_a = doc_html.DocumentNode.SelectNodes("//a").ToList();

                if (html_a.ToArray().Length > 3)
                {

                    for (var i = 2; i < html_a.ToArray().Length - 1; i++)
                    {
                        HtmlWeb website = new HtmlWeb();
                        var new_url = html_a[i].Attributes[0].Value.ToString().Split(new string[] { ";url=" }, StringSplitOptions.None);

                        try
                        {
                            HtmlDocument news_html = website.Load(new_url[1]);
                            var amazing_title = news_html.DocumentNode.SelectNodes("//title").ToList();

                            var news_related_url = new_url[1];
                            var news_related_title = amazing_title[0].InnerText;
                        }
                        catch (Exception)
                        {
                            var news_related_url = html_a[i].Attributes[0].Value;
                            var news_related_title = html_a[i].InnerText;
                        }
                        
                    }
                }

                Page.DataBind();
            }
        }
    }
}