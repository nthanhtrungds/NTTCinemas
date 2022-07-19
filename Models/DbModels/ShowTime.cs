using System;
using System.Collections.Generic;

namespace NTTCinemas.Models.DbModels
{
    public partial class ShowTime
    {
        public ShowTime()
        {
            Bookings = new HashSet<Booking>();
        }

        public int ShowTimeId { get; set; }
        public int BranchId { get; set; }
        public int RoomId { get; set; }
        public int FilmId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual Film Film { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
