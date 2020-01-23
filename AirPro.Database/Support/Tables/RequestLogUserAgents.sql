CREATE TABLE [Support].[RequestLogUserAgents] (
    [UserAgentId]     INT            IDENTITY (1, 1) NOT NULL,
    [UserAgentString] NVARCHAR (MAX) NULL,
    [UserAgentHash]   AS             (checksum([UserAgentString])),
    CONSTRAINT [PK_Support.RequestLogUserAgents] PRIMARY KEY CLUSTERED ([UserAgentId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_RequestLogUserAgents_UserAgentHash]
    ON [Support].[RequestLogUserAgents]([UserAgentHash] ASC);

