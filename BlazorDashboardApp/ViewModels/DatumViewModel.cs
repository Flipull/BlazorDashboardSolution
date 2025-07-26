using BlazorDashboardApp.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorDashboardApp.ViewModels
{
    public class DatumViewModel
    {
        public int? Id { get; set; }
        public int? SubjectId { get; set; }

        public string Filetype { get; set; }
        public string Filename { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedByUserId { get; set; }

        
        [Required(ErrorMessage = "Please upload the Data")]
        public IBrowserFile UploadableDatum { get; set; }
    }
}
