using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTTCinemas.Models.DbModels
{
    public partial class Branch
    {
        public Branch()
        {
            Rooms = new HashSet<Room>();
        }

        public int BranchId { get; set; }

        [DisplayName("Tên chi nhánh")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string BranchName { get; set; } = null!;
        
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string Address { get; set; } = null!;
                  
        
        [DisplayName("Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string PhoneNumber { get; set; } = null!;
        
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public string Email { get; set; } = null!;
        
        [DisplayName("Hình ảnh")]
        public string? Image { get; set; }
        
        [DisplayName("Quản lý")]
        public string? Manager { get; set; }
        
        [DisplayName("Mô tả")]
        public string? Description { get; set; }
        
        [DisplayName("Ngày ra mắt")]
        [Required(ErrorMessage = "Vui lòng nhập trường này.")]
        public DateTime? OpeningDate { get; set; }
        
        [DisplayName("Thời gian tạo")]
        public DateTime CreationTime { get; set; }
        
        [DisplayName("Cập nhật gần nhất")]
        public DateTime LastUpdate { get; set; }
        
        [DisplayName("Trạng thái")]
        public int Status { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
