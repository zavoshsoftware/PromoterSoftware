 

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class DailyPromoterProductSale : BaseEntity
    { 
        [Display(Name="تعداد فروش")]
        public int Count { get; set; }

        public Guid ProjectProductId { get; set; }
        public virtual ProjectProduct ProjectProduct { get; set; }

        public Guid DailyPromoterPlanId { get; set; }
        public virtual DailyPromoterPlan DailyPromoterPlan { get; set; }

        [Display(Name="توضیحات پروموتر")]
        public string PromoterDescription { get; set; }
    }
}

