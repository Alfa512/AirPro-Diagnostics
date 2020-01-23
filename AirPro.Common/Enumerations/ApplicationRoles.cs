using UniMatrix.Common.Attributes;

namespace AirPro.Common.Enumerations
{
    public enum ApplicationRoles
    {
        #region Legacy Roles

        [EnumGuid("4FD55BFD-9E79-48C8-9FB0-DB5281D5005F")]
        Administrator = 1,

        [EnumGuid("89CC2BF0-62BB-435B-8BD8-3243BE4EABE9")]
        Technician = 2,

        #endregion

        #region New Roles

        [EnumGuid("50B86018-098C-4D5D-B05F-85C1D88BF408")]
        AccountCreate,

        [EnumGuid("4E7AB500-06A1-49AB-922A-4715A58298CD")]
        AccountDelete,

        [EnumGuid("C9A8CB27-CADD-4871-9C78-8D50B3ABCE49")]
        AccountEdit,

        [EnumGuid("EEDE84E3-6F34-48AA-B729-243D9A62A7D9")]
        AccountShowAll,

        [EnumGuid("86DBFE9B-36C2-44F2-8962-8B150589F1B7")]
        AccountView,

        [EnumGuid("0C3A71B4-B4D4-4DB1-8CF1-80BD78FE522E")]
        AccountNoteCreate,

        [EnumGuid("B9DED0F5-AC46-464A-A869-2EE26346A57F")]
        AccountNoteDelete,

        [EnumGuid("D52EDEC6-911B-40F1-829A-52D30B840ED4")]
        AccountNoteEdit,

        [EnumGuid("BF4E1688-4FDE-4CE0-9AE9-0C15367CD211")]
        AccountNoteView,

        [EnumGuid("63AFF65C-A709-4BD9-8DD4-DB3549A70A75")]
        GroupCreate,

        [EnumGuid("2A8B8E10-3484-4C9C-962B-781DB4D2F01C")]
        GroupDelete,

        [EnumGuid("46972454-DDA2-492C-9AB0-54F16BF31B5E")]
        GroupEdit,

        [EnumGuid("3257AE6A-CE68-43A6-9920-3FC2A28CBCFA")]
        GroupView,

        [EnumGuid("60652BDB-C975-40F1-9C44-E8B0D07690FF")]
        GroupShowAll,

        [EnumGuid("A12950C4-B140-4C7D-B2B8-E41E3E98F842")]
        InvoiceCreate,

        [EnumGuid("C61B09A9-58E9-4C2F-8989-8809DA769E78")]
        InvoiceDelete,

        [EnumGuid("5C7C6711-1445-4D57-9714-C2B1464D0484")]
        InvoiceEdit,

        [EnumGuid("E3CA5212-B018-465B-9AF0-B29AD4C6E380")]
        InvoiceView,

        [EnumGuid("14D1EFE0-8D78-478D-B818-0FDE920BDA58")]
        InvoiceNoteCreate,

        [EnumGuid("07FB61AD-C511-437E-9CEA-14AC32D07C98")]
        InvoiceNoteDelete,

        [EnumGuid("2AE0E405-FAFB-4314-BCB6-6AC749585654")]
        InvoiceNoteEdit,

        [EnumGuid("FA4D8E30-DCBE-48D1-976E-AF00748DFE85")]
        InvoiceNoteView,

        [EnumGuid("0274AD7E-5931-49AA-B643-B12F902CBE15")]
        PaymentCreate,

        [EnumGuid("06651E34-514C-4F1A-98C0-F6F9A1F016A5")]
        PaymentDelete,

        //[EnumGuid("D06B6696-5F86-44FC-85E5-7F92995B4976")]
        //PaymentEdit,

        [EnumGuid("753BAD44-C28F-42F6-9D67-6832353D1BA5")]
        PaymentView,

        [EnumGuid("86D428C6-A95B-43F4-867C-F44652B3109E")]
        RepairCreate,

        [EnumGuid("7E9E1D77-C509-4A18-A4A7-5651D2ED3447")]
        RepairDelete,

        [EnumGuid("0EB9EDEB-5B71-40AB-8877-31027C2BEE7B")]
        RepairEdit,

        [EnumGuid("D3C6015D-6E55-4CC3-8F3B-87B7A8BB1EC7")]
        RepairView,

        [EnumGuid("7476F653-B2B9-4C71-967A-A77E36E64B9B")]
        ReportCreate,

        [EnumGuid("8BC8619C-8C0B-407D-8A6C-951CB9A84A36")]
        ReportDelete,

        [EnumGuid("B2FC0BD1-70C8-481B-8205-826E6A1907AC")]
        ReportEdit,

        [EnumGuid("F5B9CDBC-A197-4693-91F6-42283B45F7B1")]
        ReportView,

        [EnumGuid("DBBE48C6-84C4-42B6-A03A-51FAEB52AD68")]
        SupportMoveRequest,

        [EnumGuid("9FA46A27-C0B0-4502-8EE6-49C91B2A36A8")]
        SupportChangeRepairVin,

        [EnumGuid("C07BD44A-502B-4650-8572-59C41575FE18")]
        ShopCreate,

        [EnumGuid("038CE263-B64B-403D-93A6-ADC96A18E59F")]
        ShopDelete,

        [EnumGuid("CB1B5CCC-2B81-4573-8317-6994A2140D01")]
        ShopEdit,

        [EnumGuid("982C936C-149D-475D-AC0A-4DE4D7BB18D5")]
        ShopShowAll,

        [EnumGuid("C1A04D20-0D30-47A6-B566-31E8887CB46E")]
        ShopView,

        [EnumGuid("FDAA301E-7956-4AA8-BD65-1466422F3B40")]
        ShopNoteCreate,

        [EnumGuid("5595EB57-297C-4B65-BE7C-66C0D58F5ACC")]
        ShopNoteDelete,

        [EnumGuid("8B5CA041-2991-40D4-952D-8E70C51E018C")]
        ShopNoteEdit,

        [EnumGuid("CCFDB3B6-65D5-44C4-8B13-F97A7E1159E3")]
        ShopNoteView,

        [EnumGuid("F8A83C0F-EBA6-4A92-B49D-878057784DBD")]
        ShopSelfScanGrant,

        //[EnumGuid("9417C6E6-604E-4F26-B5C7-4424637761D5")]
        //SystemAdmin,

        [EnumGuid("DB17D188-5624-4C31-AF8C-45C005205D40")]
        SystemDebug,

        [EnumGuid("B7F46D0E-508F-49BB-8377-1F2546209330")]
        UserCreate,

        [EnumGuid("F1FE61EA-AB1B-4E16-B16A-7146107BF700")]
        UserDelete,

        [EnumGuid("EA333481-CAFD-44FD-990C-78FE9126A558")]
        UserEdit,

        [EnumGuid("08E2CA87-60DE-4763-99D9-BD5EC2D87DDC")]
        UserShowAll,

        [EnumGuid("DEBDBD17-1942-41E5-AE55-8152ECD920F7")]
        UserView,

        [EnumGuid("36D9E192-A307-45F8-8B79-86F28417CF2C")]
        UserNoteCreate,

        [EnumGuid("879F690E-2B4D-4509-98D1-A58D8F1A7B75")]
        UserNoteDelete,

        [EnumGuid("CEDD0071-98CA-40D8-B92B-6EF5B2968ECB")]
        UserNoteEdit,

        [EnumGuid("2C96EE58-E5BF-4053-B4B5-E0307B5AB263")]
        UserNoteView,

        [EnumGuid("0115382D-70FD-4255-BF25-42DF908B1CC6")]
        RequestTypeAdmin,

        [EnumGuid("52FC8646-8A4A-48D7-AD27-D77965BC31AB")]
        NotificationAdmin,

        [EnumGuid("AE60C601-3F9D-48E0-8AE4-C740C6A56373")]
        SystemDashboardView,

        [EnumGuid("D50E01B8-1A64-46C9-BB56-2E63EEA9E4EE")]
        SystemReportingView,

        [EnumGuid("BD595F2A-ACF1-4E0A-928C-5ADB9AD0A43F")]
        WorkTypeAdmin,

        [EnumGuid("B7D2CD05-3ADE-422F-BF2F-D2AFD4E852B2")]
        PricingPlanCreate,

        [EnumGuid("A823ADC0-5F0F-487B-8525-044232E510EE")]
        PricingPlanEdit,

        [EnumGuid("182BD96F-1677-4D3C-B18C-7BF20EE6B672")]
        PricingPlanView,

        [EnumGuid("C9A82EE0-0345-48C2-ABCD-C60D2383C10A")]
        InsuranceCoAdmin,

        [EnumGuid("18386582-57E9-4A08-B9A6-AEF5044A587A")]
        TechProfileAdmin,

        [EnumGuid("57F70092-2072-44E1-88CD-9FDD8317E35D")]
        TechProfileNoteCreate,

        [EnumGuid("8AE51835-9D27-431B-9FF0-58AE081107D5")]
        TechProfileNoteDelete,

        [EnumGuid("037FED2E-FF43-448D-86D3-1969346612A7")]
        TechProfileNoteEdit,

        [EnumGuid("0F949E24-3841-4CB8-A507-ECAA6986696D")]
        TechProfileNoteView,

        [EnumGuid("F255FB34-FDDE-4590-9E05-14D27DBE0CA5")]
        EstimatePlanEdit,

        [EnumGuid("65171699-A50F-410E-A62A-5F63E6057062")]
        EstimatePlanCreate,

        [EnumGuid("83E72DFC-D026-4BB9-8200-12F616AB79E6")]
        EstimatePlanView,

        [EnumGuid("B4557C98-C8AE-49B2-B3F4-7E238A237FB7")]
        InventoryDeviceCreate,

        [EnumGuid("9E1DAE53-9FD6-455C-8F6E-2F74844F0093")]
        InventoryDeviceEdit,

        [EnumGuid("9D8B051F-6D24-431C-88A6-62DD5DF93399")]
        InventoryDeviceView,

        [EnumGuid("4E857CD0-4ADA-4480-8178-1F5FF264F8BA")]
        InventoryAssignmentEdit,

        [EnumGuid("94742B67-BAF4-4790-A3DF-975E9C9DC89B")]
        InventoryAssignmentView,

        [EnumGuid("5D7227CF-621C-4597-959C-CAE440C664FD")]
        InventorySubscriptionEdit,

        [EnumGuid("4EEADAF9-AB3E-41C4-9AE9-F8AFF170265D")]
        InventorySubscriptionView,

        [EnumGuid("B040ADEC-6725-4386-9DB1-0B7E33E47EC4")]
        InventoryDepositEdit,

        [EnumGuid("258C6DCF-8CA1-49D7-B6A8-D3DB7ADC1B22")]
        InventoryDepositView,

        [EnumGuid("1DF9C4A3-9B07-49C9-A2A1-8DCC1790EB82")]
        InventoryNoteCreate,

        [EnumGuid("B09DCF2B-7771-42B3-A0C1-A51F1581DEF9")]
        InventoryNoteDelete,

        [EnumGuid("5511ECF5-9EA9-4153-82D7-91C887C74D4F")]
        InventoryNoteEdit,

        [EnumGuid("8AA21AE1-90CE-4577-AB74-A664F5E4E6F9")]
        InventoryNoteView,

        [EnumGuid("CF3121EC-8D7F-4C7E-BB03-8F70EEB5D672")]
        SignatureGenerator,

        [EnumGuid("4BCB1E06-32FE-4218-8394-522DF54223AB")]
        VehicleMakeAdmin,

        [EnumGuid("6BAE0541-73F2-478F-88A2-63A8B7F25F86")]
        DecisionManageView,

        [EnumGuid("F42444D8-D634-4AF9-92AB-44BAC71C2BA2")]
        DecisionManageEdit,

        [EnumGuid("99CF2F5C-6B64-4944-83CC-3E91A7F50B13")]
        ReleaseNoteCreate,

        [EnumGuid("9DD761C7-62EE-46F9-BBF1-C3D8A92C2672")]
        ReleaseNoteEdit,

        [EnumGuid("32ED9A18-5FFA-4270-8A4F-541674E057A7")]
        ReleaseNoteView,

        [EnumGuid("B6AAE703-4D79-47B3-B11C-5F202290A37C")]
        ReleaseNoteDelete,

        [EnumGuid("7391499D-7D05-4E2E-98FB-6F9CE8CD4B53")]
        RecommendationManageView,

        [EnumGuid("E28F8A54-BA31-48D8-BE4D-6F4E277DEF28")]
        RecommendationManageEdit,

        [EnumGuid("F437D30B-ADF4-43D8-A745-4B5ADBDB0B6B")]
        RegistrationView,

        [EnumGuid("6043E58B-1E7E-40DD-ACD0-9EF7BC9F6D34")]
        RegistrationEdit,

        [EnumGuid("96DD255E-4D48-494E-A8CF-9E520775CF56")]
        RegistrationCreate,

        [EnumGuid("29D19592-0224-46A6-917C-BD2C3ABEAB00")]
        AirProEmployeeAssign

        #endregion
    }
}