using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTTCinemas.Models.DbModels
{
    public partial class Room
    {
        public Room()
        {
            Seats = new HashSet<Seat>();
            ShowTimes = new HashSet<ShowTime>();
        }

        public int RoomId { get; set; }
        public int BranchId { get; set; }

        [DisplayName("Tên phòng")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string RoomName { get; set; } = null!;

        [DisplayName("Chiều rộng (Đơn vị ghế)")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public int WidthOfRoomMap { get; set; }

        [DisplayName("Chiều dài (Đơn bị ghế)")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public int LengthOfRoomMap { get; set; }

        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        public int Status { get; set; }

        [DisplayName("Thời gian tạo")]
        public DateTime CreationTime { get; set; }

        [DisplayName("Cập nhật gần nhất")]
        public DateTime LastUpdate { get; set; }

        public virtual Branch Branch { get; set; } = null!;
        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<ShowTime> ShowTimes { get; set; }
    }
}
