using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTTCinemas.Models.DbModels
{
    public partial class Film
    {
        public Film()
        {
            ShowTimes = new HashSet<ShowTime>();
        }

        public int FilmId { get; set; }

        [DisplayName("Tên phim")]        
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string FilmName { get; set; } = null!;

        [DisplayName("Thể loại")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string FilmGenre { get; set; } = null!;

        [DisplayName("Thời lượng")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public int Duration { get; set; }

        [DisplayName("Ngôn ngữ")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string Language { get; set; } = null!;

        [DisplayName("Ngày khởi chiếu")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Đạo diễn")]
        public string? Director { get; set; }

        [DisplayName("Các diễn viên")]
        public string? Actors { get; set; }

        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        [DisplayName("Trailer")]
        public string? Trailer { get; set; }

        [DisplayName("Hình ảnh")]
        public string? Image { get; set; }

        [DisplayName("Đối tượng khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string CustomerAge { get; set; } = null!;

        [DisplayName("Likes")]
        public int? Likes { get; set; }

        [DisplayName("Trạng thái")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public int Status { get; set; }

        [DisplayName("Thời gian tạo")]
        public DateTime CreationTime { get; set; }

        [DisplayName("Cập nhật gần nhất")]
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<ShowTime> ShowTimes { get; set; }
    }
}
