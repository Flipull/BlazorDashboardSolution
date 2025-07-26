using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardApp.Data
{
    public class Transcript
    {
        public int Id { get; set; }
        public int DatumId { get; set; }
        public virtual Datum Datum { get; set; }
        public string TranscriptString { get; set; }
        

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public virtual IdentityUser DeletedByUser { get; set; }
        
        public Transcript()
        {
            
        }
    }
}
