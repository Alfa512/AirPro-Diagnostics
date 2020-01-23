CREATE TABLE [Repair].[VehicleLookups] (
    [VehicleLookupId]    INT                IDENTITY (1, 1) NOT NULL,
    [VehicleVIN]         NVARCHAR (MAX)     NOT NULL,
    [Service]            INT                NOT NULL,
    [RequestBaseURL]     NVARCHAR (MAX)     NULL,
    [RequestString]      NVARCHAR (MAX)     NULL,
    [RequestMessage]     NVARCHAR (MAX)     NULL,
    [RequestSuccess]     BIT                NOT NULL,
    [ResponseStatusCode] INT                NOT NULL,
    [ResponseContent]    NVARCHAR (MAX)     NULL,
    [RequestDt]          DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_Repair.VehicleLookups] PRIMARY KEY CLUSTERED ([VehicleLookupId] ASC)
);



