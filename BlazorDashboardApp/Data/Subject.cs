﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardApp.Data
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description{ get; set; }
        public string Photofile { get; set; }

        public virtual ICollection<Datum> Datum { get; set; } = new List<Datum>();

        public Subject()
        {

            
        }
    }
}
