 

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
        public decimal Count { get; set; }

        public Guid ProjectProductId { get; set; }
        public virtual ProjectProduct ProjectProduct { get; set; }

        public Guid DailyPromoterPlanId { get; set; }
        public virtual DailyPromoterPlan DailyPromoterPlan { get; set; }
    }
}

