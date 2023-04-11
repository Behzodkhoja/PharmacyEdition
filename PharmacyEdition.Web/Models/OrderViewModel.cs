using PharmacyEdition.Domain.Entities;
using PharmacyEdition.Domain.Enums;

namespace PharmacyEdition.Web.Models
{
    public class OrderViewModel
    {
        public long Id { get; set; }
        public StatusType Status { get; set; }
        public User User { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
