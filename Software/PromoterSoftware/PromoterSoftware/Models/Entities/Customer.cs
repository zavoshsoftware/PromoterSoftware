 

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class Customer : BaseEntity
    {
        public Customer()
        {
            Projects = new List<Project>();
        }

        [Display(Name="مشتری")]
        public string Title { get; set; }

        [Display(Name="رابط")]
        public string ContactPersonFullName { get; set; }

        [Display(Name="شماره موبایل رابط")]
        public string ContactPersonCellNumber { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

    }
}

