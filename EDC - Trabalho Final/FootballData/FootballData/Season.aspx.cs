using FootballData.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FootballData
{
    public partial class Season : System.Web.UI.Page
    {
        private SqlConnection con;
        public string seasonCaption;
        public string numberOfTeams;
        public string numberOfGames;
        public string year;
        public string lastUpdated;
        public string matchday;
        public int id;

        public string leagueTable_html;
        public string matchdayTable_html;

        public string id_str;

        public int min_MatchDay, max_MatchDay;

        protected void Page_Load(object sender, EventArgs e)
        {
            con = ConnectionDB.getConnection();
            var feed_language = Languages.userLanguage(Request);
            try
            {
                id = int.Parse(Request["ID"]);
            }
            catch (Exception) { }

            // Get Season
            String CmdString1 = "SELECT * FROM football.udf_get_season(@seasonID)";
            SqlCommand cmd1 = new SqlCommand(CmdString1, con);
            cmd1.Parameters.AddWithValue("@seasonID", id);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable("season");
            sda1.Fill(dt1);
            
            seasonCaption = dt1.Rows[0].ItemArray[3].ToString();
            numberOfGames = dt1.Rows[0].ItemArray[5].ToString();
            numberOfTeams = dt1.Rows[0].ItemArray[6].ToString();
            year = dt1.Rows[0].ItemArray[7].ToString();
            lastUpdated = dt1.Rows[0].ItemArray[4].ToString().Replace("T", " ").Replace("Z", " ");

            var url = dt1.Rows[0].ItemArray[2].ToString();
            var syncClient = new WebClient();
            syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
            syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
            var content = "";
            var leagueTable = new LeagueTable();// = new LeagueTable();
            try
            {
                content = syncClient.DownloadString(url);
                leagueTable = JsonConvert.DeserializeObject<LeagueTable>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                // http://json2csharp.com/

                leagueTable_html = "";

                foreach (Standing s in leagueTable.standing)
                {
                    int idteam;
                    Int32.TryParse(s._links.team.href.ToString().Replace("http://api.football-data.org/v1/teams/", ""), out idteam);
                    String CmdString4 = "SELECT * FROM football.udf_get_teamNames(@teamID)";
                    SqlCommand cmd4 = new SqlCommand(CmdString4, con);
                    cmd4.Parameters.AddWithValue("@teamID", idteam);
                    SqlDataAdapter sda4 = new SqlDataAdapter(cmd4);
                    DataTable dt4 = new DataTable("team");
                    sda4.Fill(dt4);

                    var columnName = "name" + feed_language.ToUpper();
                    var index = dt4.Columns.IndexOf(columnName);
                    var teamName = dt4.Rows[0].ItemArray[index].ToString();
                    leagueTable_html += "<tr><td>" + s.position + "</td><td><a href=\"Team.aspx?ID=" + s._links.team.href.ToString().Replace("http://api.football-data.org/v1/teams/", "") + "\">" + teamName + "</a></td><td>" + s.points + "</td><td>" + s.playedGames + "</td><td>" + s.wins + "</td><td>" + s.draws + "</td><td>" + s.losses + "</td><td>" + s.goals + "</td><td>" + s.goalsAgainst + "</td><td>" + s.goalDifference + "</td></tr>";
                }
            }
            catch (WebException)
            {

            }
           
           

            // get fixtures
            url = dt1.Rows[0].ItemArray[1].ToString();
            syncClient = new WebClient();
            syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
            syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");

            content = syncClient.DownloadString(url);
            var fixtures = JsonConvert.DeserializeObject<Fixtures>(content);

            matchdayTable_html = "";

            min_MatchDay = fixtures.fixtures[0].matchday;
            max_MatchDay = fixtures.fixtures[fixtures.fixtures.ToArray().Length - 1].matchday;

            try
            {
                matchday = int.Parse(Request["matchday"]).ToString();
                if (int.Parse(matchday) > max_MatchDay)
                {
                    matchday = max_MatchDay.ToString();
                }
            }
            catch (Exception)
            {
                if(leagueTable.matchday == 0)
                {
                    matchday = max_MatchDay.ToString();
                }
                else
                {
                    matchday = leagueTable.matchday.ToString();
                }
                
            }
            
            foreach (Fixture fix in fixtures.fixtures)
            {
                if (fix.matchday.ToString() == matchday)
                {
                    int idteam;
                    Int32.TryParse(fix._links.homeTeam.href.ToString().Replace("http://api.football-data.org/v1/teams/", ""), out idteam);
                    String CmdString4 = "SELECT * FROM football.udf_get_teamNames(@teamID)";
                    SqlCommand cmd4 = new SqlCommand(CmdString4, con);
                    cmd4.Parameters.AddWithValue("@teamID", idteam);
                    SqlDataAdapter sda4 = new SqlDataAdapter(cmd4);
                    DataTable dt4 = new DataTable("team");
                    sda4.Fill(dt4);

                    Int32.TryParse(fix._links.awayTeam.href.ToString().Replace("http://api.football-data.org/v1/teams/", ""), out idteam);
                    CmdString4 = "SELECT * FROM football.udf_get_teamNames(@teamID)";
                    cmd4 = new SqlCommand(CmdString4, con);
                    cmd4.Parameters.AddWithValue("@teamID", idteam);
                    SqlDataAdapter sda5 = new SqlDataAdapter(cmd4);
                    DataTable dt5 = new DataTable("team");
                    sda5.Fill(dt5);

                    var columnName = "name" + feed_language.ToUpper();
                    var indexHome = dt4.Columns.IndexOf(columnName);
                    var indexAway = dt5.Columns.IndexOf(columnName);
                    var teamNameHome = dt4.Rows[0].ItemArray[indexHome].ToString();
                    var teamNameAway = dt5.Rows[0].ItemArray[indexAway].ToString();
                    matchdayTable_html += "<tr><td>"+fix.date.ToString().Replace("Z", " ").Replace("T", " ")+ "</td><td><a href=\"Team.aspx?ID=" + fix._links.homeTeam.href.ToString().Replace("http://api.football-data.org/v1/teams/", "") + "\">" + teamNameHome + "</a></td><td><a href=\"Team.aspx?ID=" + fix._links.awayTeam.href.ToString().Replace("http://api.football-data.org/v1/teams/", "") + "\">" + teamNameAway + "</a></td>";
                    if (fix.status == "FINISHED")
                    {
                        matchdayTable_html += "<td>"+fix.result.goalsHomeTeam+"-"+fix.result.goalsAwayTeam+"</td>";
                    }
                    else
                    {
                        matchdayTable_html += "<td>--</td>";
                    }
                    matchdayTable_html += "</tr>";
                }
            }

            Page.DataBind();

        }
 
    }
}