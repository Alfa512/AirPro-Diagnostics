CREATE TABLE [Service].[CCCInsuranceCompanies] (
    [CCCInsuranceCompanyId]    NVARCHAR (128) NOT NULL,
    [CCCInsuranceCompanyName]  NVARCHAR (MAX) NULL,
    [RepairInsuranceCompanyId] INT            NULL,
    CONSTRAINT [PK_Service.CCCInsuranceCompanies] PRIMARY KEY CLUSTERED ([CCCInsuranceCompanyId] ASC),
    CONSTRAINT [FK_Service.CCCInsuranceCompanies_Repair.InsuranceCompanies_RepairInsuranceCompanyId] FOREIGN KEY ([RepairInsuranceCompanyId]) REFERENCES [Repair].[InsuranceCompanies] ([InsuranceCompanyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RepairInsuranceCompanyId]
    ON [Service].[CCCInsuranceCompanies]([RepairInsuranceCompanyId] ASC);

