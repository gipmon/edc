use EDCFootball;

-- UDF for see if one team has news or not
go

-- DROP FUNCTION football.udf_team_has_news

CREATE FUNCTION football.udf_team_has_news(@team_id int, @language varchar(2))
RETURNS int
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	RETURN (SELECT COUNT(id) FROM football.teamNew WHERE team_id=@team_id and language=@language);
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

-- DROP FUNCTION football.udf_get_team_news_related

CREATE FUNCTION football.udf_user_subscribed_team(@user_id nvarchar(128), @team_id int)
RETURNS INT
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	RETURN (SELECT COUNT(id) FROM football.teamSubscribe WHERE user_id LIKE @user_id AND team_id = @team_id);
END;
