CREATE TABLE [Scan].[RequestCategoryTypes] (
    [RequestCategoryId] INT NOT NULL,
    [RequestTypeId]     INT NOT NULL,
    CONSTRAINT [PK_Scan.RequestCategoryTypes] PRIMARY KEY CLUSTERED ([RequestCategoryId] ASC, [RequestTypeId] ASC),
    CONSTRAINT [FK_Scan.RequestCategoryTypes_Scan.RequestCategories_RequestCategoryId] FOREIGN KEY ([RequestCategoryId]) REFERENCES [Scan].[RequestCategories] ([RequestCategoryId]),
    CONSTRAINT [FK_Scan.RequestCategoryTypes_Scan.RequestTypes_RequestTypeId] FOREIGN KEY ([RequestTypeId]) REFERENCES [Scan].[RequestTypes] ([RequestTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId]
    ON [Scan].[RequestCategoryTypes]([RequestTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestCategoryId]
    ON [Scan].[RequestCategoryTypes]([RequestCategoryId] ASC);

