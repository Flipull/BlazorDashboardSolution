using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorDashboardApp.ViewModels
{
    public class TranscriptViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Data Id is required")]
        public int? DatumId { get; set; }


        [Required(ErrorMessage = "Transcript is required")]
        [StringLength(128, ErrorMessage = "Transcript must be less than 128 characters")]
        public string TranscriptString { get; set; }
        

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedByUserId { get; set; }
    }
}
