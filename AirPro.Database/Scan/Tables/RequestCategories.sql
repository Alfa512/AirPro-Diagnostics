CREATE TABLE [Scan].[RequestCategories] (
    [RequestCategoryId]   INT            IDENTITY (1, 1) NOT NULL,
    [RequestCategoryName] NVARCHAR (MAX) NULL,
    [Order]               INT            NOT NULL,
    [IsActive]            BIT            NOT NULL,
    CONSTRAINT [PK_Scan.RequestCategories] PRIMARY KEY CLUSTERED ([RequestCategoryId] ASC)
);

