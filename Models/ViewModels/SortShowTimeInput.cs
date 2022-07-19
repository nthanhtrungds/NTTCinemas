using NTTCinemas.Models.DbModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTTCinemas.Models.ViewModels
{
    public class SortShowTimeInput
    {
        public int branchId { get; set; }

        [DisplayName("Từ ngày")]
        [DataType(DataType.Date)]
        public DateTime date_from { get; set; } = DateTime.Today;

        [DisplayName("Đến ngày")]
        [DataType(DataType.Date)]
        public DateTime date_to { get; set; } = DateTime.Today;

        public virtual Branch Branch { get; set; } = null!;
    }
}
