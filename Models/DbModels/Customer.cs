using System;
using System.Collections.Generic;

namespace NTTCinemas.Models.DbModels
{
    public partial class Customer
    {
        public Customer()
        {
            Bookings = new HashSet<Booking>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
