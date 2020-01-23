CREATE TABLE [Common].[Countries] (
    [CountryId]      INT            IDENTITY (1, 1) NOT NULL,
    [AlphaCode2]     NVARCHAR (2)   NOT NULL,
    [AlphaCode3]     NVARCHAR (3)   NOT NULL,
    [NumericCodeM49] INT            NOT NULL,
    [Name]           NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Common.Countries] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

