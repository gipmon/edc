using FootballData.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FootballData
{
    public partial class rss : System.Web.UI.Page
    {
        private SqlConnection con;

        public LinkedList<TeamNew> teamNews;
        public RssChannel rssChannel;
        public Hashtable teamsNames;

        protected string url;

        protected void Page_Load(object sender, EventArgs e)
        {
            con = ConnectionDB.getConnection();
            var feed_language = Languages.userLanguage(Request);
            // get user ID
            string userId = null;
            if (Request.QueryString["userId"] != null)
            {
                userId = Request["userId"];
            }
            else
            {
                throw new HttpException(401, "We need your user ID!");
            }
            
            // get team list
            string[] teamList = null;
            if (Request.QueryString["teamList"] != null)
            {
                teamList = Request["teamList"].Split(',');
            }
            else
            {
                throw new HttpException(400, "We need your teams subscribe list!");
            }

            teamsNames = new Hashtable();

            // get teams name and verify if exists
            foreach (string team in teamList)
            {
                if (team.Length != 0)
                {
                    String CmdString1 = "SELECT * FROM football.udf_get_teamNames(@teamId)";
                    SqlCommand cmd1 = new SqlCommand(CmdString1, con);
                    cmd1.Parameters.AddWithValue("@teamId", Convert.ToInt32(team));
                    SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                    DataTable dt1 = new DataTable("team");
                    sda1.Fill(dt1);

                    if (dt1.Rows.Count == 0)
                    {
                        throw new HttpException(404, "Team doesn't exists!");
                    }

                    var columnName = "name" + feed_language.ToUpper();
                    var index = dt1.Columns.IndexOf(columnName);

                    teamsNames[Convert.ToInt32(team)] = dt1.Rows[0].ItemArray[index].ToString();
                }
            }

            // verify if the user subscribe the team list

            foreach(string team in teamList)
            {
                if (team.Length != 0)
                {
                    string cmd_str = "SELECT football.udf_user_subscribed_team(@user_id, @team_id)";
                    SqlCommand cmd_subscribe = new SqlCommand(cmd_str, con);
                    cmd_subscribe.Parameters.AddWithValue("@user_id", userId);
                    cmd_subscribe.Parameters.AddWithValue("@team_id", Convert.ToInt32(team));
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
                        throw new HttpException(401, "Please subscribe!");
                    }
                }
            }

            // format, default => full
            string format = "full";
            if (Request.QueryString["format"] == "simple")
            {
                format = Request["format"];
            }
            
            // language
            string language = "pt";
            if (Request.QueryString["language"] != null && Languages.languages_name.ContainsKey(Request["language"]))
            {
                language = Request["language"];
            }

            // get content
            teamNews = new LinkedList<TeamNew>();

            string teamsList = "";

            foreach (String teamId in teamList)
            {
                if (teamId.Length != 0)
                {
                    teamsList += teamId + ",";
                }
            }

            if (teamsList.Length == 0)
            {
                teamsList = ",,";
            }
            else
            {
                teamsList = ',' + teamsList;
            }

            String CmdString = "SELECT * FROM football.udf_get_rss_news(@teamsList, @language) ORDER BY pubDate DESC";
            SqlCommand cmd = new SqlCommand(CmdString, con);
            cmd.Parameters.AddWithValue("@teamsList", teamsList);
            cmd.Parameters.AddWithValue("@language", language);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("teamNews");
            sda.Fill(dt);

            int count_news = 0;

            foreach (DataRow teamNew in dt.Rows)
            {
                TeamNew tmp = new TeamNew(teamNew, Convert.ToInt32(teamNew.ItemArray[6]));

                if((++count_news) >= 30)
                {
                    teamNews.AddLast(tmp);
                    break;
                }

                // search for related news
                String CmdString1 = "SELECT * FROM football.udf_get_team_news_related(@related_id)";
                SqlCommand cmd1 = new SqlCommand(CmdString1, con);
                cmd1.Parameters.AddWithValue("@related_id", tmp.id);
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable("relatedNews");
                sda1.Fill(dt1);

                foreach (DataRow relatedNew in dt1.Rows)
                {
                    TeamRelatedNew tmp1 = new TeamRelatedNew(relatedNew, Convert.ToInt32(teamNew.ItemArray[6]), tmp.pubDate);
                    tmp.related.AddLast(tmp1);
                    if((++count_news) >= 30){
                        break;
                    }
                }
                
                teamNews.AddLast(tmp);
                
                if (count_news >= 30)
                {
                    break;
                }
            }

            string title = "";
            foreach(string clubName in teamsNames.Values)
            {
                title += clubName + " ";
            }

            title += "RSS Feed";

            rssChannel = new RssChannel(HttpContext.Current.Request.Url.AbsoluteUri, title, language, format);
            RepeaterRSS.DataSource = teamNews;

            RepeaterRSS.DataBind();

            url = "http://" + HttpContext.Current.Request.Url.Authority + "/";
            Page.DataBind();
        }

        protected string RemoveIllegalCharacters(object input)
        {
            // cast the input to a string
            string data = input.ToString();

            // replace illegal characters in XML documents with their entity references
            data = data.Replace("&", "&amp;");
            data = data.Replace("\"", "&quot;");
            data = data.Replace("'", "&apos;");
            data = data.Replace("<", "&lt;");
            data = data.Replace(">", "&gt;");

            return data;
        }

        public class RssChannel
        {
            public string language;
            public string format;
            public string title;
            public string link;

            public RssChannel(string link, string title, string language, string format)
            {
                this.language = language;
                this.format = format;
                this.title = title;
                this.link = link;
            }
        }

        public class TeamNew
        {
            public int id { get; set; }
            public string title { get; set; }
            public string link { get; set; }
            public string description { get; set; }
            public int team_id { get; set; }
            public DateTime pubDate { get; set; }

            public LinkedList<TeamRelatedNew> related { get; set; }

            public TeamNew(DataRow dt, int teamId)
            {
                this.id = (int)dt.ItemArray[0];
                this.title = (string)dt.ItemArray[1];
                this.link = (string)dt.ItemArray[2];
                if(((string)dt.ItemArray[5]).Length != 0)
                {
                    this.description = (string)dt.ItemArray[3] + "<br/><img src=\"" + (string)dt.ItemArray[5] + "\" />";
                }
                else
                {
                    this.description = (string)dt.ItemArray[3];
                }
                this.pubDate = (DateTime)dt.ItemArray[4];
                this.team_id = (int)dt.ItemArray[6];
                this.related = new LinkedList<TeamRelatedNew>();
            }
            
        }

        public class TeamRelatedNew
        {
            public int id { get; set; }
            public string title { get; set; }
            public string link { get; set; }
            public int team_id { get; set; }
            public DateTime pubDate { get; set; }

            public TeamRelatedNew(DataRow dt, int teamId, DateTime pubDate)
            {
                this.id = (int)dt.ItemArray[0];
                this.title = (string)dt.ItemArray[1];
                this.link = (string)dt.ItemArray[2];
                this.team_id = teamId;
                this.pubDate = pubDate;
            }
        }
    }
}