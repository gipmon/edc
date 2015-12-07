using FootballData.Controllers;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            con = ConnectionDB.getConnection();
           
            // get teams that were subscribed
            String CmdString3 = "SELECT * FROM football.udf_teams_subscribed()";
            SqlCommand cmd3 = new SqlCommand(CmdString3, con);
            SqlDataAdapter sda3 = new SqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable("teams_subscribed");
            sda3.Fill(dt3);
            
            foreach (DataRow teamSubscribed in dt3.Rows)
            {

                Hashtable names = new Hashtable();
                names.Add("name", (string)teamSubscribed.ItemArray[1]);
                names.Add("namePT", (string)teamSubscribed.ItemArray[2]);
                names.Add("nameEN", (string)teamSubscribed.ItemArray[3]);
                names.Add("nameIT", (string)teamSubscribed.ItemArray[4]);
                names.Add("nameES", (string)teamSubscribed.ItemArray[5]);
                names.Add("nameFR", (string)teamSubscribed.ItemArray[6]);
                
                Int32 team_id = (Int32)teamSubscribed.ItemArray[0];

                foreach(string feed_language in Languages.languages_name.Keys)
                {
                    // google find
                    var url = "https://news.google." + Languages.domains[feed_language] + "/news/feeds?pz=1&cf=all&q=" + Server.UrlEncode(names[Languages.table_name[feed_language].ToString()].ToString()) + "&output=rss";

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

                        // store new
                        string CmdString = "football.sp_insertNew";
                        SqlCommand cmd_new = new SqlCommand(CmdString, con);
                        cmd_new.CommandType = CommandType.StoredProcedure;
                        cmd_new.Parameters.AddWithValue("@title", news_title);
                        cmd_new.Parameters.AddWithValue("@link", news_url);
                        cmd_new.Parameters.AddWithValue("@description", news_description);
                        cmd_new.Parameters.AddWithValue("@team_id", team_id);
                        cmd_new.Parameters.AddWithValue("@language", feed_language);

                        DateTime date;
                        if (!DateTime.TryParse(news_pubDate, out date))
                        {
                            // error
                            return;
                        }

                        cmd_new.Parameters.AddWithValue("@pubDate", date);

                        SqlParameter returnID = new SqlParameter("@output", SqlDbType.Int);
                        returnID.Direction = ParameterDirection.ReturnValue;
                        cmd_new.Parameters.Add(returnID);

                        try
                        {
                            con.Open();
                            cmd_new.ExecuteNonQuery();
                            var news_id = cmd_new.Parameters[6].Value;
                            con.Close();

                            // related news

                            HtmlDocument doc_html = new HtmlDocument();
                            doc_html.LoadHtml(node.Attributes[1].Value);

                            var html_a = doc_html.DocumentNode.SelectNodes("//a").ToList();

                            if (html_a.ToArray().Length > 3)
                            {

                                for (var i = 2; i < html_a.ToArray().Length - 1; i++)
                                {
                                    HtmlWeb website = new HtmlWeb();
                                    var new_url = html_a[i].Attributes[0].Value.ToString().Split(new string[] { ";url=" }, StringSplitOptions.None);

                                    string news_related_url = null;
                                    string news_related_title = null;
                                    string news_related_description = "";

                                    try
                                    {
                                        HtmlDocument news_html = website.Load(new_url[1]);
                                        var amazing_title = news_html.DocumentNode.SelectNodes("//title").ToList();

                                        var meta_tags = news_html.DocumentNode.SelectNodes("//meta").ToList();

                                        /*
                                        TO BE IMPROVED

                                        foreach(HtmlNode n in meta_tags)
                                        {
                                            foreach(HtmlAttribute attr in n.Attributes)
                                            {
                                                if (attr.Name.Equals("description", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    news_related_description = attr.Value;
                                                }
                                            }
                                        }
                                        */

                                        news_related_url = new_url[1];
                                        news_related_title = amazing_title[0].InnerText;
                                    }
                                    catch (Exception)
                                    {
                                        news_related_url = html_a[i].Attributes[0].Value;
                                        news_related_title = html_a[i].InnerText;
                                    }

                                    // store related new
                                    CmdString = "football.sp_insertRelatedNew";
                                    cmd_new = new SqlCommand(CmdString, con);
                                    cmd_new.CommandType = CommandType.StoredProcedure;
                                    cmd_new.Parameters.AddWithValue("@title", news_related_title);
                                    cmd_new.Parameters.AddWithValue("@link", news_related_url);
                                    cmd_new.Parameters.AddWithValue("@related_id", news_id);
                                    cmd_new.Parameters.AddWithValue("@team_id", team_id);

                                    try
                                    {
                                        con.Open();
                                        cmd_new.ExecuteNonQuery();

                                        con.Close();
                                    }
                                    catch (Exception exc)
                                    {
                                        con.Close();
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            con.Close();
                        }

                    }
                }
            }
            
        }
        
    }
    
}