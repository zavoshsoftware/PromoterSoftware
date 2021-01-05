 

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
        public DateTime ShiftDate { get; set; }

        [Display(Name = "ساعت شروع کار")]
        public decimal? StartHour { get; set; }
        [Display(Name = "ساعت پایان کار")]
        public decimal? FinishHour { get; set; }

        public decimal? StartLat { get; set; }
        public decimal? StartLong { get; set; }

        public decimal? FinishLat { get; set; }
        public decimal? FinishLong { get; set; }

        public virtual ICollection<DailyPromoterPlanAttachment> DailyPromoterPlanAttachments { get; set; }
        public virtual ICollection<DailyPromoterProductSale> DailyPromoterProductSales { get; set; }
        internal class Configuration : EntityTypeConfiguration<DailyPromoterPlan>
        {
            public Configuration()
            {
                HasRequired(p => p.ProjectDetailPromoter).WithMany(j => j.DailyPromoterPlans).HasForeignKey(p => p.ProjectDetailPromoterId);
            }
        }
    }
}

