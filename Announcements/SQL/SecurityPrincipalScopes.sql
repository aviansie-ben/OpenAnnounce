CREATE TABLE [dbo].[SecurityPrincipalScopes] (
    [SecurityPrincipalId] INT NOT NULL,
    [ScopeId]             INT NOT NULL,
    CONSTRAINT [PK_SecurityPrincipalScopes] PRIMARY KEY CLUSTERED ([SecurityPrincipalId] ASC, [ScopeId] ASC),
    CONSTRAINT [FK_SecurityPrincipalScopes_SecurityPrincipals] FOREIGN KEY ([SecurityPrincipalId]) REFERENCES [dbo].[SecurityPrincipals] ([Id]),
    CONSTRAINT [FK_SecurityPrincipalScopes_Scopes] FOREIGN KEY ([ScopeId]) REFERENCES [dbo].[Scopes] ([Id])
);
