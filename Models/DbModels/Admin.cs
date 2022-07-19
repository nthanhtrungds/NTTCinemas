using System;
using System.Collections.Generic;

namespace NTTCinemas.Models.DbModels
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
    }
}
