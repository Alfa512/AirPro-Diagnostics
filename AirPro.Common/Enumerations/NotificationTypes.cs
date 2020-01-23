using UniMatrix.Common.Attributes;

namespace AirPro.Common.Enumerations
{
    public enum NotificationTypes
    {

        [EnumGuid("C9147DE5-7D32-49A3-970E-D47DF564B51F")]
        ScanRequestCreated,

        [EnumGuid("4D3889F1-0606-4A43-BB42-4ACBBB4EBBCC")]
        ScanRequestCompleted,

        [EnumGuid("1500D680-C514-4FCA-849F-64724A6BCC6D")]
        RepairCreated,

        [EnumGuid("34911271-B0E8-43E7-8F72-EBF3A59F255C")]
        RepairCompleted,

        [EnumGuid("217B3307-94A9-4DB6-A86A-B289CA639BC1")]
        InvoiceCompleted,

        [EnumGuid("9DEE850C-6866-4253-B24D-F30A46F3D4C7")]
        AccountCreated,

        [EnumGuid("BD2A511F-0808-460C-9AD9-921593F360E0")]
        ShopCreated,

        [EnumGuid("48B44FAF-6F82-4005-8400-51159121F365")]
        UserCreated,

        [EnumGuid("70F13DD4-A9AF-42D6-AE88-67104AB875A9")]
        UserLogin
    }
}
