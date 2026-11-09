
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RaceDayMVC.Models
{
    public class EventViewModel
    {
        [JsonPropertyName("eventId")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required]
        [Display(Name = "Event Date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }
            = DateTime.Today.AddDays(7);

        [Required]
        public string Location { get; set; } = "";

        [Range(0, 1000000)]
        [Display(Name = "Entry Fee (R)")]
        public decimal EntryFee { get; set; }

        [Required]
        public string Status { get; set; } = "Upcoming";
    }
}
