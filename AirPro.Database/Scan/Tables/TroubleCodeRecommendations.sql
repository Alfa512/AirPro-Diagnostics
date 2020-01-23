CREATE TABLE [Scan].[TroubleCodeRecommendations] (
    [TroubleCodeRecommendationId]   INT                IDENTITY (1, 1) NOT NULL,
    [TroubleCodeRecommendationText] NVARCHAR (MAX)     NULL,
    [CreatedByUserGuid]             UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]                     DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]             UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                     DATETIMEOFFSET (7) NULL,
    [TroubleCodeRecommendationHash] AS                 (checksum([TroubleCodeRecommendationText])),
    [ActiveInd]                     BIT                DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Scan.TroubleCodeRecommendations] PRIMARY KEY CLUSTERED ([TroubleCodeRecommendationId] ASC),
    CONSTRAINT [FK_Scan.TroubleCodeRecommendations_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.TroubleCodeRecommendations_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_TroubleCodeRecommendations_TroubleCodeRecommendationHash]
    ON [Scan].[TroubleCodeRecommendations]([TroubleCodeRecommendationHash] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[TroubleCodeRecommendations]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[TroubleCodeRecommendations]([CreatedByUserGuid] ASC);

