using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballData.Controllers
{

    public class Links
    {
        public Self self { get; set; }
        public Soccerseason soccerseason { get; set; }
    }

    public class Team
    {
        public string href { get; set; }
    }

    public class Links2
    {
        public Team team { get; set; }
    }

    public class Home
    {
        public int goals { get; set; }
        public int goalsAgainst { get; set; }
        public int wins { get; set; }
        public int draws { get; set; }
        public int losses { get; set; }
    }

    public class Away
    {
        public int goals { get; set; }
        public int goalsAgainst { get; set; }
        public int wins { get; set; }
        public int draws { get; set; }
        public int losses { get; set; }
    }

    public class Standing
    {
        public Links2 _links { get; set; }
        public int position { get; set; }
        public string teamName { get; set; }
        public int playedGames { get; set; }
        public int points { get; set; }
        public int goals { get; set; }
        public int goalsAgainst { get; set; }
        public int goalDifference { get; set; }
        public int wins { get; set; }
        public int draws { get; set; }
        public int losses { get; set; }
        public Home home { get; set; }
        public Away away { get; set; }
    }

    public class LeagueTable
    {
        public Links _links { get; set; }
        public string leagueCaption { get; set; }
        public int matchday { get; set; }
        public List<Standing> standing { get; set; }
    }
}