using FootballData.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public string seasonCaption;
        public string numberOfTeams;
        public string numberOfGames;
        public string year;
        public string lastUpdated;
        public string matchday;

        public string leagueTable_html;
        public string matchdayTable_html;

        public string id_str;

        public int min_MatchDay, max_MatchDay;

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = 0;
            try
            {
                id = int.Parse(Request["ID"]);
            }
            catch (Exception) {}

            id_str = id.ToString();
            
            SeasonClass selectedSeason = SiteMaster.seasonsList[id];

            seasonCaption = selectedSeason.caption.ToString();
            numberOfGames = selectedSeason.numberOfGames.ToString();
            numberOfTeams = selectedSeason.numberOfTeams.ToString();
            year = selectedSeason.year.ToString();
            lastUpdated = selectedSeason.lastUpdated.ToString().Replace("T", " ").Replace("Z", " ");

            var url = selectedSeason._links.leagueTable.href.ToString();
            var syncClient = new WebClient();
            syncClient.Headers.Add("X-Auth-Token", "9cf843e4d69b4817ba99eba1ea051c10");
            syncClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
            
            var content = syncClient.DownloadString(url);
            var leagueTable = JsonConvert.DeserializeObject<LeagueTable>(content);
            // http://json2csharp.com/
            
            leagueTable_html = "";

            foreach(Standing s in leagueTable.standing)
            {
                leagueTable_html += "<tr><td>"+s.position+ "</td><td><a href=\"Team.aspx?ID="+s._links.team.href.ToString().Replace("http://api.football-data.org/v1/teams/", "")+"\">" + s.teamName + "</a></td><td>" + s.points + "</td><td>" + s.playedGames + "</td><td>" + s.wins + "</td><td>" + s.draws + "</td><td>" + s.losses + "</td><td>" + s.goals + "</td><td>" + s.goalsAgainst + "</td><td>" + s.goalDifference + "</td></tr>";
            }

            // get fixtures
            url = selectedSeason._links.fixtures.href.ToString();
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
                matchday = leagueTable.matchday.ToString();
            }
            
            foreach (Fixture fix in fixtures.fixtures)
            {
                if (fix.matchday.ToString() == matchday)
                {
                    matchdayTable_html += "<tr><td>"+fix.date.ToString().Replace("Z", " ").Replace("T", " ")+ "</td><td><a href=\"Team.aspx?ID=" + fix._links.homeTeam.href.ToString().Replace("http://api.football-data.org/v1/teams/", "") + "\">" + fix.homeTeamName + "</a></td><td><a href=\"Team.aspx?ID=" + fix._links.awayTeam.href.ToString().Replace("http://api.football-data.org/v1/teams/", "") + "\">" + fix.awayTeamName + "</a></td>";
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