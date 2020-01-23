CREATE TABLE [Common].[NoteTypes] (
    [NoteTypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [NoteTypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Common.NoteTypes] PRIMARY KEY CLUSTERED ([NoteTypeId] ASC)
);

