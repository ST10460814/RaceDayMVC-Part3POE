
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class ResultViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Registration ID")]
        public int RegistrationId { get; set; }

        [Required]
        [Display(Name = "Finish Time")]
        public string FinishTime { get; set; } = "";

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Finishing Position")]
        public int FinishingPosition { get; set; }

        [Required]
        [Display(Name = "Result Status")]
        public string ResultStatus { get; set; } = "Official";
    }
}
