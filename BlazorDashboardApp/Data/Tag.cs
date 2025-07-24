using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardApp.Data
{
    public class Tag
    {
        public int Id { get; set; }
        public int DataId { get; set; }
        public virtual Data Data { get; set; }
        public string TagString { get; set; }


        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public virtual IdentityUser DeletedByUser { get; set; }


        public Tag()
        {
            
        }
    }
}
