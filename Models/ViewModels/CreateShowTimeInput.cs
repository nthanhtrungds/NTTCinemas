using NTTCinemas.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace NTTCinemas.Models.ViewModels
{
    public class CreateShowTimeInput
    {
        public List<int> BranchIds { get; set; } = null!;
        public List<int> FilmIds { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; } = DateTime.Today;
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; } = DateTime.Today;
    }
}
