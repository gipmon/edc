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
  	IF @id is null OR @link_fixtures_href is null OR @link_players_href is null OR @link_self_href is null OR @name is null OR @crestURL is null
  	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN
  	END
	
	DECLARE @count int

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

go

  CREATE PROCEDURE football.sp_associateTeamToSeason
    @seasonID				INT,
    @teamID					INT
  WITH ENCRYPTION
  AS
  	IF @seasonID is null OR @teamID is null
  	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN
  	END
	
	DECLARE @count int

	SELECT @count = count(id) FROM football.team WHERE id = @teamID;

	IF @count = 0
	BEGIN
		RETURN
	END

	SELECT @count = count(id) FROM football.season WHERE id = @seasonID;

	IF @count = 0
	BEGIN
		RETURN
	END

	SELECT @count = count(seasonID) FROM football.teamSeason WHERE seasonID = @seasonID and teamID = @teamID;

	IF @count != 0
	BEGIN
		RETURN
	END

  	BEGIN TRANSACTION;

  	BEGIN TRY
  		INSERT INTO football.teamSeason
  					([seasonID],
  					 [teamID])
  		VALUES      ( @seasonID,
  					  @teamID)

  		COMMIT TRANSACTION;
  	END TRY
  	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the association!', 14, 1)
  		ROLLBACK TRANSACTION;
  	END CATCH;

	 go
 
  CREATE PROCEDURE football.sp_createPlayer
    @name text,
    @position text,
    @jerseyNumber int,
    @dateOfBirth			text,
    @nationality		text,
    @contractUntil				text,
    @marketValue	text
   
  WITH ENCRYPTION
  AS
  	IF @name is null 
  	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN
  	END
	
	DECLARE @count int

	SELECT @count = count(*) FROM football.player WHERE player.name LIKE @name AND player.dateOfBirth LIKE @dateOfBirth 
							AND player.nationality LIKE @nationality;

	IF @count != 0
	BEGIN
		RETURN
	END

  	BEGIN TRANSACTION;

  	BEGIN TRY
  		INSERT INTO football.player
  					([name],
  					 [position],
  					 [jerseyNumber],
  					 [dateOfBirth],
  					 [nationality],
  					 [contractUntil],
  					 [marketValue])
  		VALUES      ( @name,
  					  @position,
  					  @jerseyNumber,
  					  @dateOfBirth,
  					  @nationality,
  					  @contractUntil,
  					  @marketValue)

  		COMMIT TRANSACTION;
  	END TRY
  	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the player!', 14, 1)
  		ROLLBACK TRANSACTION;
  	END CATCH;

	go

  CREATE PROCEDURE football.sp_associatePlayerToTeam
    @playerName				text,
	@playerNationality		text,
	@playerDateOfBirth		text,
    @teamID					INT
  WITH ENCRYPTION
  AS
  	IF @teamID is null OR @playerName is null OR @playerNationality is null OR @playerDateOfBirth is null
  	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN
  	END
	
	DECLARE @id int

	SELECT @id = player.id FROM football.player WHERE name LIKE @playerName AND nationality LIKE @playerNationality AND dateOfBirth LIKE @playerDateOfBirth;

	DECLARE @count int
	
	SELECT @count = count(id) FROM football.player WHERE id = @id
	IF @count = 0
	BEGIN
		RETURN
	END

	SELECT @count = count(id) FROM football.team WHERE id = @teamID;

	IF @count = 0
	BEGIN
		RETURN
	END

	SELECT @count = count(playerID) FROM football.teamPlayer WHERE teamID = @teamID and playerID = @id;

	IF @count != 0
	BEGIN
		RETURN
	END

  	BEGIN TRANSACTION;

  	BEGIN TRY
  		INSERT INTO football.teamPlayer
  					([playerID],
  					 [teamID])
  		VALUES      ( @id,
  					  @teamID)

  		COMMIT TRANSACTION;
  	END TRY
  	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the teamPlayer!', 14, 1)
  		ROLLBACK TRANSACTION;
  	END CATCH;

go


CREATE PROCEDURE football.sp_insertRelatedNew
	@title					text,
	@link					text,
	@related_id				int,
	@team_id				int
	WITH ENCRYPTION
	AS
	IF @title is null OR @link is null OR @related_id is null OR @team_id is null 
	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN
	END
	
	DECLARE @count int

	SELECT @count = count(id) FROM football.team_related_new WHERE link = @link;

	IF @count = 0
	BEGIN
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
  		INSERT INTO football.team_related_new
  						([title],
  						 [link],
						 [related_id],
						 [team_id])
  		VALUES      (	@title,
  						@link,
						@related_id,
						@team_id)

  		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the new!', 14, 1)
  		ROLLBACK TRANSACTION;
	END CATCH;
