using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballData.Controllers
{

    public class Players
    {
        public string href { get; set; }
    }

    public class FixturesUrl
    {
        public string href { get; set; }
    }

    public class LinksTeam
    {
        public Self self { get; set; }
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
}