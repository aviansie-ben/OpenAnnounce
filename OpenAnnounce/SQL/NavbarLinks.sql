CREATE TABLE [dbo].[NavbarLinks] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Text]  VARCHAR (32)  NOT NULL,
    [URL]   VARCHAR (128) NOT NULL,
    [Scope] INT           CONSTRAINT [DF_NavbarLinks_Scope] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_NavbarLinks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NavbarLinks_Scopes] FOREIGN KEY ([Scope]) REFERENCES [dbo].[Scopes] ([Id])
);
