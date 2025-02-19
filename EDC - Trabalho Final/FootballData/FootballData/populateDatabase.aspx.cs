﻿using System;
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
using MicrosoftTranslatorSdk.HttpSamples;
using System.IO;

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
            for (int i = 2012; i < 2013; i++)
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
                        string translationDEPT = translate(t.name, "de", "pt");
                        cmd_season.Parameters.AddWithValue("@namePT", translationDEPT);
                        string translationDEEN = translate(t.name, "de", "en");
                        cmd_season.Parameters.AddWithValue("@nameEN", translationDEEN);
                        string translationDEIT = translate(t.name, "de", "it");
                        cmd_season.Parameters.AddWithValue("@nameIT", translationDEIT);
                        string translationDEES = translate(t.name, "de", "es");
                        cmd_season.Parameters.AddWithValue("@nameES", translationDEES);
                        string translationDEFR = translate(t.name, "de", "fr");
                        cmd_season.Parameters.AddWithValue("@nameFR", translationDEFR);
                        if (t.code == null)
                        {
                            t.code = " ";
                        }
                        cmd_season.Parameters.AddWithValue("@code", t.code);
                        if (t.shortName == null)
                        {
                            t.shortName = " ";
                        }
                        cmd_season.Parameters.AddWithValue("@shortName", t.shortName);
                        if (t.squadMarketValue == null)
                        {
                            t.squadMarketValue = " ";
                        }
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

                        url = "http://api.football-data.org/v1/teams/" + id + "/players";

                        syncClient = new WebClient();
                        syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
                        syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                        content = syncClient.DownloadString(url);

                        PlayersList playerList;
                        playerList = JsonConvert.DeserializeObject<PlayersList>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        foreach (Player p in playerList.players)
                        {
                            CmdString = "football.sp_createPlayer";
                            cmd_season = new SqlCommand(CmdString, con);
                            cmd_season.CommandType = CommandType.StoredProcedure;

                            cmd_season.Parameters.AddWithValue("@name", p.name);
                           
                            if (p.dateOfBirth == null)
                            {
                                p.dateOfBirth = " ";
                            }
                            cmd_season.Parameters.AddWithValue("@dateOfBirth", p.dateOfBirth);
                            if (p.nationality == null)
                            {
                                p.nationality = " ";
                            }
                            cmd_season.Parameters.AddWithValue("@nationality", p.nationality);
                           
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

                            // associate player to team
                            CmdString = "football.sp_associatePlayerToTeam";
                            cmd_season = new SqlCommand(CmdString, con);
                            cmd_season.CommandType = CommandType.StoredProcedure;
                            cmd_season.Parameters.AddWithValue("@name", p.name);
                            if (p.nationality == null)
                            {
                                p.nationality = " ";
                            }
                            cmd_season.Parameters.AddWithValue("@nationality", p.nationality);
                            if (p.dateOfBirth == null)
                            {
                                p.dateOfBirth = " ";
                            }
                            cmd_season.Parameters.AddWithValue("@dateOfBirth", p.dateOfBirth);
                            cmd_season.Parameters.AddWithValue("@jerseyNumber", p.jerseyNumber);
                            if (p.contractUntil == null)
                            {
                                p.contractUntil = " ";
                            }
                            cmd_season.Parameters.AddWithValue("@contractUntil", p.contractUntil);
                            if (p.marketValue == null)
                            {
                                p.marketValue = " ";
                            }
                            cmd_season.Parameters.AddWithValue("@marketValue", p.marketValue);
                            if (p.position == null)
                            {
                                p.position = " ";
                            }
                            cmd_season.Parameters.AddWithValue("@position", p.position);
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
            }
            

        }

        protected string translate(string teamName, string f, string t)
        {
            string text = teamName;
            string from = f;
            string to = t;
            GetAccessToken admToken = new GetAccessToken();
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;
            string authToken = admToken.getHeader();

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);

            WebResponse response = null;
            string translation;
            response = httpWebRequest.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                translation = (string)dcs.ReadObject(stream);
            }
            return translation;
        }
    }
}