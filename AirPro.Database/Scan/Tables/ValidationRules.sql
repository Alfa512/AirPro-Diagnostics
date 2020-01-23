CREATE TABLE [Scan].[ValidationRules] (
    [ValidationRuleId]        INT             IDENTITY (1, 1) NOT NULL,
    [ValidationRuleText]      NVARCHAR (100)  NOT NULL,
    [ValidationRuleDetails]   NVARCHAR (1000) NULL,
    [ValidationRuleSortOrder] INT             NOT NULL,
    [ValidationRuleActiveInd] BIT             DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Scan.ValidationRules] PRIMARY KEY CLUSTERED ([ValidationRuleId] ASC)
);

