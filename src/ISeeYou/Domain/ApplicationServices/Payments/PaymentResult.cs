using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISeeYou.Domain.ApplicationServices.Payments
{
    public class PaymentResult
    {
        public string SubscriptionId { get; set; }
        public string CreditCardId { get; set; }
        public string CustomerId { get; set; }

        public bool Success { get; set; }

        public IEnumerable<String> Errors { get; set; }
    }
}
