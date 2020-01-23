﻿CREATE TYPE [Access].[udt_ShopContacts] AS TABLE (
    [ShopContactGuid] UNIQUEIDENTIFIER NULL,
    [ShopGuid]        UNIQUEIDENTIFIER NULL,
    [FirstName]       NVARCHAR (MAX)   NULL,
    [LastName]        NVARCHAR (MAX)   NULL,
    [PhoneNumber]     NVARCHAR (MAX)   NULL);

