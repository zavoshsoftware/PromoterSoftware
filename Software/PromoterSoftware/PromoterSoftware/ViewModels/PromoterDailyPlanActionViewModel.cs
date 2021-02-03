using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class PromoterDailyPlanActionViewModel
    {
        public string ProjectTitle { get; set; }
        public string BrandTitle { get; set; }
        public string StoreTitle { get; set; }
        public string SupervisorFullname { get; set; }
        public string Description { get; set; }
        public List<string> ProjectAttachments { get; set; }
        public List<ProjectProductItem> ProjectProducts { get; set; }
        public List<DailyPromoterPlanAttachment> DailyPromoterPlanAttachments { get; set; }
        public bool IsStart { get; set; }
        public bool IsFinish { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string ShiftDate { get; set; }
    }

    public class ProjectProductItem
    {
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ProductDescription { get; set; }
        public int? Count { get; set; }
        public string PromoterDesc { get; set; }

    }
}