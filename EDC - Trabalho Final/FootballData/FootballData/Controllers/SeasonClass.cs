using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class SeasonClass
{

    public Links _links { get; set; }
    public string caption { get; set; }
    public string league { get; set; }
    public string year { get; set; }
    public int numberOfTeams { get; set; }
    public int numberOfGames { get; set; }
    public string lastUpdated { get; set; }

    public class Self
    {
        public string href { get; set; }
    }

    public class Teams
    {
        public string href { get; set; }
    }

    public class Fixtures
    {
        public string href { get; set; }
    }

    public class LeagueTable
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public Teams teams { get; set; }
        public Fixtures fixtures { get; set; }
        public LeagueTable leagueTable { get; set; }
    }

    public SeasonClass()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}
