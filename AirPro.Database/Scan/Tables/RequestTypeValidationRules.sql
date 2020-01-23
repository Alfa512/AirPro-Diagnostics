CREATE TABLE [Scan].[RequestTypeValidationRules] (
    [RequestTypeId]    INT NOT NULL,
    [ValidationRuleId] INT NOT NULL,
    CONSTRAINT [PK_Scan.RequestTypeValidationRules] PRIMARY KEY CLUSTERED ([RequestTypeId] ASC, [ValidationRuleId] ASC),
    CONSTRAINT [FK_Scan.RequestTypeValidationRules_Scan.RequestTypes_RequestTypeId] FOREIGN KEY ([RequestTypeId]) REFERENCES [Scan].[RequestTypes] ([RequestTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Scan.RequestTypeValidationRules_Scan.ValidationRules_ValidationRuleId] FOREIGN KEY ([ValidationRuleId]) REFERENCES [Scan].[ValidationRules] ([ValidationRuleId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ValidationRuleId]
    ON [Scan].[RequestTypeValidationRules]([ValidationRuleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId]
    ON [Scan].[RequestTypeValidationRules]([RequestTypeId] ASC);

