using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Notifications.WebJob.Models
{
    public class StatementNotificationModel
    {
        public int PaymentId { get; set; }
        public Guid ShopGuid { get; set; }
        public string ShopName { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentReferenceNumber { get; set; }
        public string PaymentMemo { get; set; }

        public string PaymentType { get; set; }
        public string PaymentCurrency { get; set; }
        public int DiscountPercentage { get; set; }
    }
}
