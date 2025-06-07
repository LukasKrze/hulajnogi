using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Models
{
    public class TopTenViewModel
    {
        public ReportMode Mode { get; set; }

        [Display(Name = "Rok")]
        public int? Year { get; set; }

        [Display(Name = "Dane od")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? From { get; set; }

        [Display(Name = "Dane do")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? To { get; set; }
    }

    public enum ReportMode
    {
        [Display(Name = "Cały rok")]
        WholeYear = 1,
        Q1 = 2,
        Q2 = 3,
        Q3 = 4,
        Q4 = 5,
        [Display(Name = "Własny przedział")]
        CustomRange = 6
    }
}
