CREATE FULLTEXT INDEX ON [Repair].[vwOrderDetails]
    ([SearchText] LANGUAGE 1033)
    KEY INDEX [PK_RepairOrderDetails]
    ON [AirProSearchCatalog];


GO
CREATE FULLTEXT INDEX ON [Scan].[vwRequestDetails]
    ([SearchText] LANGUAGE 1033)
    KEY INDEX [PK_ScanRequestDetails]
    ON [AirProSearchCatalog];

