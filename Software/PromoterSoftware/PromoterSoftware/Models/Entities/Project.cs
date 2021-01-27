

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
        [UIHint("PersianDatePicker")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        [UIHint("PersianDatePicker")]
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



        #region NotMapped

        [Display(Name = "تاریخ شروع")]
        [NotMapped]
        public string StartDateStr
        {
            get
            {
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(StartDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(StartDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(StartDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day );
            }
        }

        [Display(Name = "تاریخ پایان")]
        [NotMapped]
        public string EndDateStr
        {
            get
            {
                if (EndDate == null)
                    return string.Empty;

                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(EndDate.Value).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(EndDate.Value).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(EndDate.Value).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day );
            }
        }

        #endregion
    }
}

