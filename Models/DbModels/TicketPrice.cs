using System;
using System.Collections.Generic;

namespace NTTCinemas.Models.DbModels
{
    public partial class TicketPrice
    {
        public TicketPrice()
        {
            Bookings = new HashSet<Booking>();
        }

        public int TicketPriceId { get; set; }
        public string SeatType { get; set; } = null!;
        public bool IsWeekend { get; set; }
        public bool IsHotTime { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
