using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballData.Controllers
{

    public class TeamHref
    {
        public string href { get; set; }
    }

    public class LinksPlayers
    {
        public Self _self { get; set; }
        public TeamHref team { get; set; }
    }

    public class Player
    {
        public string name { get; set; }
        public string position { get; set; }
        public int jerseyNumber { get; set; }
        public string dateOfBirth { get; set; }
        public string nationality { get; set; }
        public string contractUntil { get; set; }
        public string marketValue { get; set; }
    }

    public class PlayersList
    {
        public LinksPlayers _links { get; set; }
        public int count { get; set; }
        public List<Player> players { get; set; }
    }
}