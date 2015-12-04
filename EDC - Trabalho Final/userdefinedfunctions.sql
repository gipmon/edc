use EDCFootball;

-- UDF for see if one team has news or not
go

CREATE FUNCTION football.udf_team_has_news(@team_id int)
RETURNS int
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	RETURN (SELECT COUNT(id) FROM football.teamNew WHERE team_id=@team_id);
END;

go;

-- DROP FUNCTION football.udf_get_team_news

CREATE FUNCTION football.udf_get_team_news(@team_id int)
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
					WHERE	team_id = @team_id;
	RETURN;
END;

go;
SELECT * FROM football.udf_get_team_news(1) ORDER BY pubDate DESC;