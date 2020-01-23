CREATE TABLE [Repair].[PointOfImpacts] (
    [PointOfImpactId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_Repair.PointOfImpacts] PRIMARY KEY CLUSTERED ([PointOfImpactId] ASC)
);

