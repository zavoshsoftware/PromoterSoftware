 

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class DailyPromoterPlanAttachment : BaseEntity
    { 
        [Display(Name="ضمیمه")]
        public string FileUrl { get; set; }

        public Guid DailyPromoterPlanId { get; set; }
        public virtual DailyPromoterPlan DailyPromoterPlan { get; set; }
    }
}

