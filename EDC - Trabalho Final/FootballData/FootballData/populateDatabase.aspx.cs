using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using FootballData.Controllers;

namespace FootballData
{
    public partial class populateDatabase : System.Web.UI.Page
    {
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<SeasonClass> seasonsList;

            con = ConnectionDB.getConnection();
            
            // Populate Seasons
            for (int i = 2010; i < 2016; i++)
            {
                var url = "http://api.football-data.org/v1/soccerseasons/?season=" + i;
                var syncClient = new WebClient();
                syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
                syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                var content = syncClient.DownloadString(url);

                WebHeaderCollection headers = syncClient.ResponseHeaders;
                String reset = syncClient.ResponseHeaders[3];

                seasonsList = JsonConvert.DeserializeObject<List<SeasonClass>>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                foreach (SeasonClass s in seasonsList)
                {
                    string CmdString = "football.sp_createSeason";
                    SqlCommand cmd_season = new SqlCommand(CmdString, con);
                    cmd_season.CommandType = CommandType.StoredProcedure;
                    String[] tmp = s._links.self.href.ToString().Split('/');
                    int season_id;
                    if (!Int32.TryParse(tmp[tmp.Length - 1], out season_id))
                    {
                        return;
                    }
                    cmd_season.Parameters.AddWithValue("@id", season_id);
                    cmd_season.Parameters.AddWithValue("@link_fixtures_href", s._links.fixtures.href);
                    cmd_season.Parameters.AddWithValue("@link_leagueTable_href", s._links.leagueTable.href);
                    cmd_season.Parameters.AddWithValue("@link_self_href", s._links.self.href);
                    cmd_season.Parameters.AddWithValue("@link_teams_href", s._links.teams.href);
                    cmd_season.Parameters.AddWithValue("@caption", s.caption);
                    cmd_season.Parameters.AddWithValue("@last_updated", s.lastUpdated);
                    cmd_season.Parameters.AddWithValue("@league", s.league);
                    cmd_season.Parameters.AddWithValue("@numberOfGames", s.numberOfGames);
                    cmd_season.Parameters.AddWithValue("@numberOfTeams", s.numberOfTeams);
                    cmd_season.Parameters.AddWithValue("@seasonYear", s.year);

                    try
                    {
                        con.Open();
                        cmd_season.ExecuteNonQuery();

                        con.Close();
                    }
                    catch (Exception exc)
                    {
                        con.Close();
                    }

                    url = "http://api.football-data.org/v1/soccerseasons/" + season_id + "/teams";

                    syncClient = new WebClient();
                    syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
                    syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                    content = syncClient.DownloadString(url);
                    headers = syncClient.ResponseHeaders;
                    reset = syncClient.ResponseHeaders[3];

                    TeamList teamList;
                    teamList = JsonConvert.DeserializeObject<TeamList>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    foreach (TeamClass t in teamList.teams)
                    {
                        CmdString = "football.sp_createTeam";
                        cmd_season = new SqlCommand(CmdString, con);
                        cmd_season.CommandType = CommandType.StoredProcedure;
                        tmp = t._links.self.href.ToString().Split('/');

                        int id;

                        if (!Int32.TryParse(tmp[tmp.Length - 1], out id))
                        {
                            return;
                        }
                        cmd_season.Parameters.AddWithValue("@id", id);
                        cmd_season.Parameters.AddWithValue("@link_fixtures_href", t._links.fixtures.href);
                        cmd_season.Parameters.AddWithValue("@link_players_href", t._links.players.href);
                        cmd_season.Parameters.AddWithValue("@link_self_href", t._links.self.href);
                        cmd_season.Parameters.AddWithValue("@name", t.name);
                        cmd_season.Parameters.AddWithValue("@code", t.code);
                        cmd_season.Parameters.AddWithValue("@shortName", t.shortName);
                        cmd_season.Parameters.AddWithValue("@squadMarketValue", t.squadMarketValue);
                        cmd_season.Parameters.AddWithValue("@crestURL", t.crestUrl);

                        try
                        {
                            con.Open();
                            cmd_season.ExecuteNonQuery();

                            con.Close();
                        }
                        catch (Exception exc)
                        {
                            con.Close();
                        }

                        // associate team to season
                        CmdString = "football.sp_associateTeamToSeason";
                        cmd_season = new SqlCommand(CmdString, con);
                        cmd_season.CommandType = CommandType.StoredProcedure;
                        cmd_season.Parameters.AddWithValue("@seasonID", season_id);
                        cmd_season.Parameters.AddWithValue("@teamID", id);

                        try
                        {
                            con.Open();
                            cmd_season.ExecuteNonQuery();

                            con.Close();
                        }
                        catch (Exception exc)
                        {
                            con.Close();
                        }
                    }
                    System.Threading.Thread.Sleep(1500);
                    
                }
            }

            //Populate Teams
            

        }
    }
}