using System;
using System.Collections.Generic;

namespace NTTCinemas.Models.DbModels
{
    public partial class Receipt
    {
        public Receipt()
        {
            Bookings = new HashSet<Booking>();
        }

        public int ReceiptId { get; set; }
        public int TotalPrice { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public int PaymentStatus { get; set; }
        public DateTime PaymentTime { get; set; }
        public DateTime CreationRime { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
