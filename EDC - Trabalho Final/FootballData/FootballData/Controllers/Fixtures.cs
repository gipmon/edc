using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballData.Controllers
{

    public class Link
    {
        public Self self { get; set; }
        public Soccerseason soccerseason { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Soccerseason
    {
        public string href { get; set; }
    }

    public class HomeTeam
    {
        public string href { get; set; }
    }

    public class AwayTeam
    {
        public string href { get; set; }
    }

    public class LinksFixture
    {
        public Self self { get; set; }
        public Soccerseason soccerseason { get; set; }
        public HomeTeam homeTeam { get; set; }
        public AwayTeam awayTeam { get; set; }
    }

    public class Result
    {
        public int? goalsHomeTeam { get; set; }
        public int? goalsAwayTeam { get; set; }
    }

    public class Fixture
    {
        public LinksFixture _links { get; set; }
        public string date { get; set; }
        public string status { get; set; }
        public int matchday { get; set; }
        public string homeTeamName { get; set; }
        public string awayTeamName { get; set; }
        public Result result { get; set; }
    }

    public class Fixtures
    {
        public Link _links { get; set; }
        public int count { get; set; }
        public List<Fixture> fixtures { get; set; }
    }

}