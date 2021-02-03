

using System.Data.Entity.ModelConfiguration;

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class DailyPromoterPlan : BaseEntity
    {
        public DailyPromoterPlan()
        {
            DailyPromoterPlanAttachments = new List<DailyPromoterPlanAttachment>();
            DailyPromoterProductSales = new List<DailyPromoterProductSale>();
        }

        [Display(Name = "پروموتر - پروژه")]
        public Guid ProjectDetailPromoterId { get; set; }
        public virtual ProjectDetailPromoter ProjectDetailPromoter { get; set; }

        [Display(Name = "روز کاری")]
        [UIHint("PersianDatePicker")]
        public DateTime ShiftDate { get; set; }


        [Display(Name = "ساعت شروع کار")]
        public int? StartHour { get; set; }
        [Display(Name = "دقیقه شروع کار")]
        public int? StartMin { get; set; }

        [Display(Name = "ساعت پایان کار")]
        public int? FinishHour { get; set; }
        [Display(Name = "دقیقه پایان کار")]
        public int? FinishMin { get; set; }




        public string StartLat { get; set; }
        public string StartLong { get; set; }

        public string FinishLat { get; set; }
        public string FinishLong { get; set; }

        public virtual ICollection<DailyPromoterPlanAttachment> DailyPromoterPlanAttachments { get; set; }
        public virtual ICollection<DailyPromoterProductSale> DailyPromoterProductSales { get; set; }
        internal class Configuration : EntityTypeConfiguration<DailyPromoterPlan>
        {
            public Configuration()
            {
                HasRequired(p => p.ProjectDetailPromoter).WithMany(j => j.DailyPromoterPlans).HasForeignKey(p => p.ProjectDetailPromoterId);
            }
        }


        [Display(Name = "تاریخ ایجاد")]
        [NotMapped]
        public string ShiftDateStr
        {
            get
            {
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(ShiftDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(ShiftDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(ShiftDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day);
            }
        }

        [NotMapped]
        [Display(Name = "ساعت شروع کار")]
        public string StartHourStr
        {
            get
            {
                if (StartHour != null)
                    return StartHour + ":" + StartMin;
                return "-";
            }
        }


        [Display(Name = "ساعت پایان کار")]
        [NotMapped]
        public string FinishHourStr
        {
            get
            {
                if (FinishHour != null)
                    return FinishHour + ":" + FinishMin;
                return "-";

            }
        }


    }
}

