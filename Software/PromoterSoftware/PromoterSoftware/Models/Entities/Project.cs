

using System.Web.Mvc;

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class Project : BaseEntity
    {
        public Project()
        {
            ProjectDetails=new List<ProjectDetail>();
            ProjectAttachments = new List<ProjectAttachment>();
            ProjectProducts = new List<ProjectProduct>();
        }

        [Display(Name = "عنوان پروژه")]
        public string Title { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "مشتری")]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [AllowHtml]
        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
        public virtual ICollection<ProjectAttachment> ProjectAttachments { get; set; }
        public virtual ICollection<ProjectProduct> ProjectProducts { get; set; }
    }
}

