using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardApp.Data
{
    public class Datum
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public DateTime CreationDate { get; set; };

        public string Filetype { get; set; }
        public string Filename { get; set; }

        public virtual ICollection<Transcript> Transcript { get; set; } = new List<Transcript>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public virtual IdentityUser DeletedByUser { get; set; }

        public Datum()
        {
            
        }

    }
}
