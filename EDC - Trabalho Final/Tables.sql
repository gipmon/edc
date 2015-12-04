--Create Database EDCFootball;


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
	dateOfBirth text,
	nationality text
);

CREATE TABLE football.teamplayer(
	playerID int REFERENCES football.player(id),
	teamID int REFERENCES football.team(id),
	position text,
	jerseyNumber INT,
	contractUntil text,
	marketValue text
);

-- DROP TABLE football.teamNew
-- DROP TABLE football.teamRelatedNew

CREATE TABLE football.teamNew(
	id INT UNIQUE IDENTITY,
	title text,
	link varchar(350),
	description text,
	language varchar(2),
	team_id int REFERENCES football.team(id),
	pubDate datetime,
	PRIMARY KEY(link, team_id)
);

CREATE TABLE football.teamRelatedNew(
	id INT UNIQUE IDENTITY,
	title text,
	link varchar(350),
	related_id int REFERENCES football.teamNew(id),
	team_id int REFERENCES football.team(id),
	PRIMARY KEY(link, team_id)
);

go;

CREATE TABLE football.teamSubscribe(
	id INT UNIQUE IDENTITY,
	user_id nvarchar(128) REFERENCES dbo.AspNetUsers(id) ,
	team_id int REFERENCES football.team(id),
	PRIMARY KEY(user_id, team_id)
);


