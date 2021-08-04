CREATE DATABASE Casino
GO

USE Casino
GO

CREATE SCHEMA Casino
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Casino].[Bet]'))
DROP TABLE [Casino].[Bet]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Casino].[Roulette]'))
DROP TABLE [Casino].[Roulette]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Casino].[State]'))
DROP TABLE [Casino].[State]
GO


CREATE TABLE [Casino].[State] (
    Id INT NOT NULL,
    Name VARCHAR(30) NOT NULL,
    CONSTRAINT PK_State PRIMARY KEY (Id)
);
GO

INSERT INTO [Casino].[State] VALUES (1, 'Open');
INSERT INTO [Casino].[State] VALUES (2, 'Close');
INSERT INTO [Casino].[State] VALUES (3, 'Pending');
GO

CREATE TABLE [Casino].[Roulette] (
    Id NVARCHAR(50) NOT NULL,
    StateId INT NOT NULL,
    CreationDate DATETIME NOT NULL,
    ClosedDate DATETIME NULL,
    CONSTRAINT PK_Roulette PRIMARY KEY (Id),
    CONSTRAINT FK_Roulette_State FOREIGN KEY (StateId) 
    REFERENCES [Casino].[State](Id)
);
GO

INSERT INTO [Casino].[Roulette] VALUES (NEWID(), 3, GETDATE(), NULL);
GO

CREATE TABLE [Casino].[Bet] (
    Id NVARCHAR(50) NOT NULL,
    RouletteId NVARCHAR(50) NOT NULL,
    UserId VARCHAR(30) NOT NULL,
    Number INTEGER NULL,
    Color VARCHAR (30) NULL,
    Quantity DECIMAL(5,0) NOT NULL,
    TotalWinner DECIMAL(7,0) NULL,
    CONSTRAINT PK_Bet PRIMARY KEY (Id),
    CONSTRAINT FK_Bet_Roulette FOREIGN KEY (RouletteId) 
    REFERENCES [Casino].[Roulette](Id)
);
GO

CREATE OR ALTER PROCEDURE [Casino].[SaveRoulette](
    @RouletteId NVARCHAR(50)
) AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRAN;
        INSERT INTO [Casino].[Roulette] VALUES (@RouletteId, 3, GETDATE(), NULL);
    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE [Casino].[SaveBet](
    @RouletteId NVARCHAR(50),
    @UserId VARCHAR(30),
    @Number INT NULL,
    @Color VARCHAR(30) NULL,
    @Quantity DECIMAL(7,0),
    @Result INT OUTPUT
) AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRAN;

        SET @Result = 1; -- Ok
        IF EXISTS(SELECT 1 FROM [Casino].[Roulette] R WHERE R.Id = @RouletteId)
            BEGIN
                IF NOT EXISTS(SELECT 1 FROM [Casino].[Roulette] R WHERE R.Id = @RouletteId AND R.StateId <> 3)
                    SET @Result = 2;  -- Roulette NOT OPEN
            END
        ELSE
            SET @Result = 3; -- Not exist
        
        IF(@Result = 1)
            BEGIN
                INSERT INTO [Casino].[Bet] 
                VALUES (NEWID(), @RouletteId, @UserId, @Number, @Color, @Quantity, NULL);
            END
    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE [Casino].[GetRouletteBetById] (
    @RouletteId NVARCHAR(50)
)
AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    BEGIN TRAN;
        SELECT R.Id AS RouletteId, B.Id AS Id, B.Number, B.Color, B.Quantity, B.UserId
        FROM [Casino].[Roulette] R
        INNER JOIN [Casino].[Bet] B ON R.Id = B.RouletteId
        WHERE R.Id = @RouletteId
    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE [Casino].[CloseRoulette](
    @RouletteId NVARCHAR(50),
    @Result INT OUTPUT
) AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRY
        BEGIN TRAN;
            SET @Result = 1; -- Ok
            IF EXISTS(SELECT 1 FROM [Casino].[Roulette] R WHERE R.Id = @RouletteId)
                BEGIN
                    IF EXISTS(SELECT 1 FROM [Casino].[Roulette] R WHERE R.Id = @RouletteId AND R.StateId = 2)
                        SET @Result = 3;  -- Bet already closed
                END
            ELSE
                SET @Result = 4; -- Not exist
            
            IF(@Result = 1)
                BEGIN
                    UPDATE [Casino].[Roulette]
                    SET StateId = 2, ClosedDate = GETDATE()
                    WHERE Id = @RouletteId
                END
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        SET @Result = 3;
        RETURN @Result;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [Casino].[OpenRoulette](
    @RouletteId NVARCHAR(50),
    @Result INT OUTPUT
) AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRY
        BEGIN TRAN;
            SET @Result = 1; -- Ok
            IF EXISTS(SELECT 1 FROM [Casino].[Roulette] R WHERE R.Id = @RouletteId)
                BEGIN
                    IF EXISTS(SELECT 1 FROM [Casino].[Roulette] R WHERE R.Id = @RouletteId AND R.StateId = 1)
                        SET @Result = 2;  -- Roulette alredy open
                    ELSE
                        BEGIN
                            IF EXISTS(SELECT 1 FROM [Casino].[Roulette] R WHERE R.Id = @RouletteId AND R.StateId = 2)
                                SET @Result = 3;  -- Roulette is closed
                        END
                END
            ELSE
                SET @Result = 4; -- Not exist
            
            IF(@Result = 1)
                BEGIN
                    UPDATE [Casino].[Roulette]
                    SET StateId = 1
                    WHERE Id = @RouletteId
                END
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        -- ROLLBACK TRAN;
        SET @Result = 2;
        RETURN @Result;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [Casino].[UpdateBet](
    @RouletteId NVARCHAR(50),
    @BetId NVARCHAR(50),
    @TotalWinner DECIMAL(7,0)
) AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRAN;
        UPDATE [Casino].[Bet]
        SET TotalWinner = @TotalWinner
        WHERE RouletteId = @RouletteId AND Id = @BetId;
    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE [Casino].[GetRoulette]
AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    BEGIN TRAN;
        SELECT R.Id AS Id, S.Name AS State, R.CreationDate, R.ClosedDate
        FROM [Casino].[Roulette] R
        INNER JOIN [Casino].[State] S ON R.StateId = S.Id
    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE [Casino].[GetRouletteById] (
    @RouletteId NVARCHAR(50)
)
AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    BEGIN TRAN;
        SELECT R.Id, S.Name AS State, R.CreationDate, R.ClosedDate
        FROM [Casino].[Roulette] R
        INNER JOIN [Casino].[State] S ON R.StateId = S.Id
        WHERE R.Id = @RouletteId
    COMMIT TRAN;
END
GO