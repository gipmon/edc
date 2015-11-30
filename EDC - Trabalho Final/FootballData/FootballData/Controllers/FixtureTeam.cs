using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballData.Controllers
{

    public class SelfFixture
    {
        public string href { get; set; }
    }

    public class TeamFixture
    {
        public string href { get; set; }
    }

    public class LinksFixture3
    {
        public SelfFixture _self { get; set; }
        public TeamFixture team { get; set; }
    }

    public class Self2
    {
        public string href { get; set; }
    }

    public class SoccerseasonFixture
    {
        public string href { get; set; }
    }

    public class HomeTeamFixture
    {
        public string href { get; set; }
    }

    public class AwayTeamFixture
    {
        public string href { get; set; }
    }

    public class LinksFixture2
    {
        public Self2 self { get; set; }
        public SoccerseasonFixture soccerseason { get; set; }
        public HomeTeamFixture homeTeam { get; set; }
        public AwayTeamFixture awayTeam { get; set; }
    }

    public class ResultFixture
    {
        public int? goalsHomeTeam { get; set; }
        public int? goalsAwayTeam { get; set; }
    }

    public class FixtureGame
    {
        public LinksFixture2 _links { get; set; }
        public string date { get; set; }
        public string status { get; set; }
        public int matchday { get; set; }
        public string homeTeamName { get; set; }
        public string awayTeamName { get; set; }
        public ResultFixture result { get; set; }
    }

    public class FixtureTeam
    {
        public LinksFixture3 _links { get; set; }
        public int count { get; set; }
        public List<FixtureGame> fixtures { get; set; }
    }

}