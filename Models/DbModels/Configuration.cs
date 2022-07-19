using System;
using System.Collections.Generic;

namespace NTTCinemas.Models.DbModels
{
    public partial class Configuration
    {
        public int ConfigurationId { get; set; }
        public string ConfigurationName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Value { get; set; }
    }
}
