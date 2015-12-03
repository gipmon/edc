use EDCFootball;

Create Schema football;
CREATE TABLE football.season(
	id INT PRIMARY KEY,
	link_fixtures_href text,
	link_leagueTable_href text,
	link_self_href text,
	link_teams_href text,
	caption text,
	last_updated text,
	league text,
	numberOfGames INT,
	numberOfTeams INT,
	seasonYear text
);

CREATE TABLE football.team(
	id INT PRIMARY KEY,
	link_fixtures_href text,
	link_players_href text,
	link_self_href text,
	name text,
	code text,
	shortName text,
	squadMarketValue text,
	crestURL text
);

CREATE TABLE football.teamSeason(
	seasonID INT REFERENCES football.season(id),
	teamID INT REFERENCES football.team(id)
);

CREATE TABLE football.player(
	id INT PRIMARY KEY IDENTITY,
	name text,
	position text,
	jerseyNumber INT,
	dateOfBirth text,
	nationality text,
	contractUntil text,
	marketValue text
);

CREATE TABLE football.teamplayer(
	playerID int REFERENCES football.player(id),
	seasonID int REFERENCES football.season(id)
);

CREATE TABLE football.teamSubscription(
	userID NVARCHAR(128) REFERENCES dbo.AspNetUsers(Id),
	teamID int REFERENCES football.team(id)
);

CREATE TABLE football.team_new(
	id INT PRIMARY KEY IDENTITY,
	title text,
	link text,
	description INT,
	team_id int REFERENCES football.team(id)
);

CREATE TABLE football.team_related_new(
	id INT PRIMARY KEY IDENTITY,
	title text,
	link text,
	related_id int REFERENCES football.team_new(id),
	team_id int REFERENCES football.team(id)
);



