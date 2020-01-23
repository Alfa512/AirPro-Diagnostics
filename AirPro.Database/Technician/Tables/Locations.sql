CREATE TABLE [Technician].[Locations] (
    [LocationId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (MAX) NULL,
    [SortOrder]  INT            NOT NULL,
    CONSTRAINT [PK_Technician.Locations] PRIMARY KEY CLUSTERED ([LocationId] ASC)
);

