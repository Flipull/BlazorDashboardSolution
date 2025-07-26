using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace BlazorDashboardApp.ViewModels
{
    public class SubjectViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(64, ErrorMessage = "Name must be less than 64 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(512, ErrorMessage = "Description must be less than 512 characters")]
        public string Description { get; set; }
        public string Photofile { get; set; }

        [Required(ErrorMessage = "Please upload a Photo")]
        public IBrowserFile UploadablePhoto { get; set; }
    }
}
