use EDCFootball;

-- UDF for see if one team has news or not
go

-- DROP FUNCTION football.udf_team_has_news

CREATE FUNCTION football.udf_team_has_news(@team_id int)
RETURNS int
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	RETURN (SELECT COUNT(id) FROM football.teamNew WHERE team_id=@team_id);
END;

go;

-- DROP FUNCTION football.udf_get_team_news

CREATE FUNCTION football.udf_get_team_news(@team_id int, @language varchar(2))
RETURNS @table TABLE ("id" int, "title" text, "link" varchar(350), "description" text, "pubDate" datetime)
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	INSERT @table SELECT	id,
							title, 
							link, 
							description,
							pubDate
					FROM	football.teamNew
					WHERE	team_id = @team_id AND language = @language;
	RETURN;
END;

go;
SELECT * FROM football.udf_get_team_news(1) ORDER BY pubDate DESC;

go;

-- DROP FUNCTION football.udf_get_team_news_related

CREATE FUNCTION football.udf_get_team_news_related(@related_id int)
RETURNS @table TABLE ("id" int, "title" text, "link" varchar(350))
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	INSERT @table SELECT	id,
							title, 
							link
					FROM	football.teamRelatedNew
					WHERE	related_id = @related_id;
	RETURN;
END;

go;

-- DROP FUNCTION football.udf_get_season2015_names

CREATE FUNCTION football.udf_get_season2015_names()
RETURNS @table TABLE ("caption" text, "id" int)
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	INSERT @table SELECT	caption, id
					FROM	football.season
					WHERE	seasonYear LIKE '2015';
	RETURN;
END;
GO
SELECT * FROM football.udf_get_season2015_names();

-- DROP FUNCTION football.udf_get_season

CREATE FUNCTION football.udf_get_season(@seasonID int)
RETURNS @table TABLE ("id" int, "link_fixtures_href" text, "link_leagueTable_href" text,
						"caption" text, "last_updated" text, "numberOfGames" int, "numberOfTeams" int,
						"seasonYear" text)
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	INSERT @table SELECT	id, link_fixtures_href, link_leagueTable_href, caption,
							last_updated, numberOfGames, numberOfTeams, seasonYear
					FROM	football.season
					WHERE	id = @seasonID;
	RETURN;
END;
GO

-- DROP FUNCTION football.udf_get_players

CREATE FUNCTION football.udf_get_players(@teamID int)
RETURNS @table TABLE ("id" int, "name" text, "dateOfBirth" text,
						"nationality" text, "teamID" int, "position" text, "jerseyNumber" int,
						"contractUntil" text, "marketValue" text)
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	INSERT @table SELECT id, name, dateOfBirth, nationality, 
						 teamplayer.teamID, position, jerseyNumber, 
						 contractUntil, marketValue
				  FROM (football.player JOIN football.teamplayer 
				  ON player.id = teamplayer.playerID) 
				  WHERE teamplayer.teamID = @teamID;

	RETURN;
END;
GO
SELECT * FROM football.udf_get_players(503);

-- DROP FUNCTION football.udf_get_team

CREATE FUNCTION football.udf_get_team(@teamID int)
RETURNS @table TABLE ("id" int, "name" text, "squadMarketValue" text,
						"crestURL" text, "link_fixtures_href" text)
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	INSERT @table SELECT id, name, squadMarketValue, crestURL, link_fixtures_href		 
				  FROM football.team   
				  WHERE team.id = @teamID;

	RETURN;
END;
GO
SELECT * FROM football.udf_get_team(503);

-- DROP FUNCTION football.udf_get_leagues

CREATE FUNCTION football.udf_get_leagues(@teamID int)
RETURNS @table TABLE ("id" int, "caption" text, "seasonYear" text)
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	INSERT @table SELECT season.id, caption, seasonYear		 
				  FROM (football.teamSeason JOIN football.team ON teamSeason.teamID = team.id) JOIN
				  football.season ON teamSeason.seasonID = season.id 
				  WHERE team.id = @teamID;

	RETURN;
END;
GO
SELECT * FROM football.udf_get_team(503);

