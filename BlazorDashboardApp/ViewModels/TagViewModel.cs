using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorDashboardApp.ViewModels
{
    public class TagViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Data Id is required")]
        public int? DatumId { get; set; }


        [Required(ErrorMessage = "Tag is required")]
        [StringLength(32, ErrorMessage = "Tag must be less than 32 characters")]
        public string TagString { get; set; }
        

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedByUserId { get; set; }
    }
}
