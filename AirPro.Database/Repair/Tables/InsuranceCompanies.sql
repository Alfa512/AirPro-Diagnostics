CREATE TABLE [Repair].[InsuranceCompanies] (
    [InsuranceCompanyId]   INT            IDENTITY (1, 1) NOT NULL,
    [InsuranceCompanyName] NVARCHAR (200) NULL,
    [ProgramName]          NVARCHAR (128) NULL,
    [DisabledInd]          BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Repair.InsuranceCompanies] PRIMARY KEY CLUSTERED ([InsuranceCompanyId] ASC)
);






GO

