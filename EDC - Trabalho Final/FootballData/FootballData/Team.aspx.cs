using FootballData.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FootballData
{
    public partial class Team : System.Web.UI.Page
    {

        public TeamClass team;
        public PlayersList players_list;
        public String players_list_html;
        public String fixturesTable_html;

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
            players_list = JsonConvert.DeserializeObject<PlayersList>(content);

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

            Page.DataBind();
        }
    }
}