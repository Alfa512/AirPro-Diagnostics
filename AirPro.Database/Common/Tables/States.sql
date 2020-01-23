CREATE TABLE [Common].[States] (
    [StateId]      INT            IDENTITY (1, 1) NOT NULL,
    [CountryId]    INT            NOT NULL,
    [Abbreviation] NVARCHAR (2)   NOT NULL,
    [Name]         NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Common.States] PRIMARY KEY CLUSTERED ([StateId] ASC),
    CONSTRAINT [FK_Common.States_Common.Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Common].[Countries] ([CountryId])
);


GO
CREATE NONCLUSTERED INDEX [IX_CountryId]
    ON [Common].[States]([CountryId] ASC);

