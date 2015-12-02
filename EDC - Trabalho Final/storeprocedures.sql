use EDCFootball;

go

-- DROP PROC football.sp_createSeason

CREATE PROCEDURE football.sp_createSeason
  @id				INT,
  @link_fixtures_href text,
  @link_leagueTable_href text,
  @link_self_href text,
  @link_teams_href text,
  @caption			text,
  @last_updated		text,
  @league				text,
  @numberOfGames	int,
  @numberOfTeams	int,
  @seasonYear		text
WITH ENCRYPTION
AS
	IF @id is null OR @link_fixtures_href is null OR @link_leagueTable_href is null OR @link_self_href is null OR @link_teams_href is null OR
		@caption is null OR @last_updated is null OR @league is null OR @numberOfGames is null OR
		@numberOfTeams is null OR @seasonYear is null
	BEGIN
		PRINT 'Any field can not be null!'
		RETURN
	END
	DECLARE @count int;
	SELECT @count = count(id) FROM football.season WHERE id = @id;

	IF @count != 0
	BEGIN
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
		INSERT INTO football.season
					([id],
					 [link_fixtures_href],
					 [link_leagueTable_href],
					 [link_self_href],
					 [link_teams_href],
					 [caption],
					 [last_updated],
					 [league],
					 [numberOfGames],
					 [numberOfTeams],
					 [seasonYear])
		VALUES      ( @id,
					  @link_fixtures_href,
					  @link_leagueTable_href,
					  @link_self_href,
					  @link_teams_href,
					  @caption,
					  @last_updated,
					  @league,
					  @numberOfGames,
					  @numberOfTeams,
					  @seasonYear)

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		RAISERROR ('An error occurred when creating the season!', 14, 1)
		ROLLBACK TRANSACTION;
	END CATCH;

  go
 
  CREATE PROCEDURE football.sp_createTeam
    @id				INT,
    @link_fixtures_href text,
    @link_players_href text,
    @link_self_href text,
    @name			text,
    @code		text,
    @shortName				text,
    @squadMarketValue	text,
    @crestURL	text
  WITH ENCRYPTION
  AS
  	IF @id is null OR @link_fixtures_href is null OR @link_players_href is null OR @link_self_href is null OR @name is null OR
  		@code is null OR @shortName is null OR @squadMarketValue is null OR @crestURL is null
  	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN
  	END
	
	DECLARE @count int

	-- check if the department exists
	SELECT @count = count(id) FROM football.team WHERE id = @id;

	IF @count != 0
	BEGIN
		RETURN
	END

  	BEGIN TRANSACTION;

  	BEGIN TRY
  		INSERT INTO football.team
  					([id],
  					 [link_fixtures_href],
  					 [link_players_href],
  					 [link_self_href],
  					 [name],
  					 [code],
  					 [shortName],
  					 [squadMarketValue],
  					 [crestURL])
  		VALUES      ( @id,
  					  @link_fixtures_href,
  					  @link_players_href,
  					  @link_self_href,
  					  @name,
  					  @code,
  					  @shortName,
  					  @squadMarketValue,
  					  @crestURL)

  		COMMIT TRANSACTION;
  	END TRY
  	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the team!', 14, 1)
  		ROLLBACK TRANSACTION;
  	END CATCH;
