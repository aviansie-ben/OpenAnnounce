﻿CREATE TABLE [dbo].[Clubs] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50) NOT NULL,
    [Description]   TEXT          NOT NULL,
    [Location]      NVARCHAR (50) NOT NULL,
    [Teacher]       INT           NOT NULL,
    [Weekday]       INT           NOT NULL,
    [AfterSchool]   BIT           NOT NULL,
    [CreateTime]    DATETIME      NOT NULL,
    [CreateUser]    INT           NOT NULL,
    [EditTime]      DATETIME      NOT NULL,
    [EditUser]      INT           NULL,
    [StatusTime]    DATETIME      NOT NULL,
    [StatusUser]    INT           NULL,
    [StatusMessage] VARCHAR (50)  NULL,
    [Status]        INT           NOT NULL,
    CONSTRAINT [PK_Clubs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Clubs_Users_Teacher] FOREIGN KEY ([Teacher]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Clubs_Users_Create] FOREIGN KEY ([CreateUser]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Clubs_Users_Edit] FOREIGN KEY ([EditUser]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Clubs_Users_Status] FOREIGN KEY ([StatusUser]) REFERENCES [dbo].[Users] ([Id])
);