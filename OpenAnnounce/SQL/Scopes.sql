CREATE TABLE [dbo].[Scopes] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Scopes] PRIMARY KEY CLUSTERED ([Id] ASC)
);
