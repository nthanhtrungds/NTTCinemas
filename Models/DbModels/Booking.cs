using System;
using System.Collections.Generic;

namespace NTTCinemas.Models.DbModels
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int ShowTimeId { get; set; }
        public int FilmId { get; set; }
        public int BranchId { get; set; }
        public int RoomId { get; set; }
        public int SeatId { get; set; }
        public int TicketPriceId { get; set; }
        public int CustomerId { get; set; }
        public int Price { get; set; }
        public DateTime BookingTime { get; set; }
        public int BookingStatus { get; set; }
        public int? ReceiptId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Receipt? Receipt { get; set; }
        public virtual ShowTime ShowTime { get; set; } = null!;
        public virtual TicketPrice TicketPrice { get; set; } = null!;
    }
}
