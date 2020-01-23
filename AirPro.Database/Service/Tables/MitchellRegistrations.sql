CREATE TABLE [Service].[MitchellRegistrations] (
    [MitchellRegistrationId] INT                IDENTITY (1, 1) NOT NULL,
    [MitchellAccountId]      NVARCHAR (MAX)     NULL,
    [CallbackUrl]            NVARCHAR (MAX)     NULL,
    [UserEmail]              NVARCHAR (MAX)     NULL,
    [RequestDt]              DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [CallbackPerformedDt]    DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Service.MitchellRegistrations] PRIMARY KEY CLUSTERED ([MitchellRegistrationId] ASC)
);

