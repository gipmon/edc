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
	@namePT			text,
	@nameEN			text,
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
					 [namePT],
					 [nameEN],
  					 [code],
  					 [shortName],
  					 [squadMarketValue],
  					 [crestURL])
  		VALUES      ( @id,
  					  @link_fixtures_href,
  					  @link_players_href,
  					  @link_self_href,
  					  @name,
					  @namePT,
					  @nameEN,
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
    @dateOfBirth			text,
    @nationality		text
   
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
  					 [dateOfBirth],
  					 [nationality])
  		VALUES      ( @name,
  					  @dateOfBirth,
  					  @nationality)

  		COMMIT TRANSACTION;
  	END TRY
  	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the player!', 14, 1)
  		ROLLBACK TRANSACTION;
  	END CATCH;

	go

  CREATE PROCEDURE football.sp_associatePlayerToTeam
    @name					text,
	@nationality			text,
	@dateOfBirth			text,
	@jerseyNumber			int,
	@position				text,
	@contractUntil			text,
	@marketValue			text,
    @teamID					INT
  WITH ENCRYPTION
  AS
  	IF @teamID is null OR @name is null OR @nationality is null OR @dateOfBirth is null OR @contractUntil is null
	OR @jerseyNumber is null OR @marketValue is null
  	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN
  	END
	
	DECLARE @id int

	SELECT @id = player.id FROM football.player WHERE name LIKE @name AND nationality LIKE @nationality AND dateOfBirth LIKE @dateOfBirth
															
	PRINT @id;
	DECLARE @count int
	
	SELECT @count = count(id) FROM football.player WHERE id = @id
	PRINT @count;
	IF @count = 0
	BEGIN
		RETURN
	END

	SELECT @count = count(id) FROM football.team WHERE id = @teamID;
	PRINT @count;
	IF @count = 0
	BEGIN
		RETURN
	END

	SELECT @count = count(playerID) FROM football.teamPlayer WHERE teamID = @teamID and playerID = @id;
	PRINT @count;
	IF @count != 0
	BEGIN
		RETURN
	END
	PRINT @teamID;

  	BEGIN TRANSACTION;

  	BEGIN TRY
  		INSERT INTO football.teamPlayer
  					([playerID],
  					 [teamID],
					 [position],
					 [jerseyNumber],
					 [contractUntil],
					 [marketValue])
  		VALUES      ( @id,
  					  @teamID,
					  @position,
					  @jerseyNumber,
					  @contractUntil,
					  @marketValue)

  		COMMIT TRANSACTION;
  	END TRY
  	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the teamPlayer!', 14, 1)
  		ROLLBACK TRANSACTION;
  	END CATCH;

go

--EXEC football.sp_associatePlayerToTeam @playerName = 'Manuel Neuer', @playerNationality = 'Germany', @playerDateOfBirth = '1986-03-27',
			--							@playerJerseyNumber = 1, @playerContractUntil = '2019-06-30', @playerMarketValue = '45,000,000 �', @teamID = 5;

CREATE PROCEDURE football.sp_insertRelatedNew
	@title					text,
	@link					varchar(350),
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

	SELECT @count = count(id) FROM football.teamRelatedNew WHERE link like @link AND team_id = @team_id;

	IF @count = 1
	BEGIN
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
  		INSERT INTO football.teamRelatedNew
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


go

-- DROP PROCEDURE football.sp_insertNew

CREATE PROCEDURE football.sp_insertNew
	@title					text,
	@link					varchar(350),
	@description			text,
	@language				varchar(2),
	@pubDate				datetime,
	@team_id				int
	WITH ENCRYPTION
	AS
	IF @title is null OR @link is null OR @description is null OR @team_id is null 
	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN -1
	END
	
	DECLARE @count int;
	DECLARE @returnvalue INT;

	SELECT @count = count(id) FROM football.teamNew WHERE link like @link AND team_id = @team_id AND language = @language;

	IF @count = 1
	BEGIN
		SET @returnvalue = (SELECT TOP 1 id FROM football.teamNew WHERE link like @link AND team_id = @team_id AND language = @language ORDER BY id DESC);
		RETURN @returnvalue;
	END
	

	BEGIN TRANSACTION;

	BEGIN TRY
  		INSERT INTO football.teamNew
  						([title],
  						[link],
						[description],
						[team_id],
						[language],
						[pubDate])
  		VALUES      (	@title,
  						@link,
						@description,
						@team_id,
						@language,
						@pubDate);
		
  		COMMIT TRANSACTION;
		SET @returnvalue = (SELECT TOP 1 id FROM football.teamNew ORDER BY id DESC);
		RETURN @returnvalue;
  	END TRY
	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the new!', 14, 1)
  		ROLLBACK TRANSACTION;
	END CATCH;

-- DROP PROCEDURE football.sp_toggleSubscription

CREATE PROCEDURE football.sp_toggleSubscription
	@user_id				nvarchar(128),
	@team_id				int
	WITH ENCRYPTION
	AS
	IF @user_id is null OR @team_id is null
	BEGIN
  		PRINT 'Any field can not be null!'
  		RETURN
	END
	
	DECLARE @count int

	SELECT @count = count(id) FROM football.team WHERE id = @team_id;
	
	IF @count = 0
	BEGIN
		RETURN
	END

	SELECT @count = count(userID) FROM football.teamSubscription WHERE userID LIKE @user_id AND teamID = @team_id;

	IF @count = 1
	BEGIN
		BEGIN TRANSACTION;

		BEGIN TRY
			DELETE FROM football.teamSubscription WHERE userID LIKE @user_id AND teamID = @team_id;
  			COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
  			RAISERROR ('An error occurred when creating the new!', 14, 1)
  			ROLLBACK TRANSACTION;
		END CATCH;
		RETURN
	END
	-- else
	BEGIN TRANSACTION;

	BEGIN TRY
  		INSERT INTO football.teamSubscription
  						([userId],
  						 [teamId])
  		VALUES      (	@user_id,
						@team_id)

  		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
  		RAISERROR ('An error occurred when creating the new!', 14, 1)
  		ROLLBACK TRANSACTION;
	END CATCH;

-- go
-- USE EDCFootball;
-- EXEC football.sp_toggleSubscription 'bf4e4333-bef2-4727-9d9f-c2342b46d656', 