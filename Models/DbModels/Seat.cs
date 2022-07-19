using System;
using System.Collections.Generic;

namespace NTTCinemas.Models.DbModels
{
    public partial class Seat
    {
        public int SeatId { get; set; }
        public int RoomId { get; set; }
        public int BranhId { get; set; }
        public int AxisX { get; set; }
        public int AxisY { get; set; }
        public string SeatLabel { get; set; } = null!;
        public string? Note { get; set; }
        public int BranchId { get; set; }

        public virtual Room Room { get; set; } = null!;
    }
}
