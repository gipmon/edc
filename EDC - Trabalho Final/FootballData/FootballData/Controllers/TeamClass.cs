using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballData.Controllers
{
    public class LinkAll
    {
        public string self { get; set; }
        public string soccerseason { get; set; }
    }

    public class Players
    {
        public string href { get; set; }
    }

    public class SelfTeam
    {
        public string href { get; set; }
    }

    public class FixturesUrl
    {
        public string href { get; set; }
    }

    public class LinksTeam
    {
        public SelfTeam self { get; set; }
        public FixturesUrl fixtures { get; set; }
        public Players players { get; set; }
    }

    public class TeamClass
    {
        public LinksTeam _links { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string shortName { get; set; }
        public string squadMarketValue { get; set; }
        public string crestUrl { get; set; }
    }

    public class TeamList
    {
        public List<LinkAll> _links { get; set; }
        public int count { get; set; }
        public List<TeamClass> teams { get; set; }
    }
}