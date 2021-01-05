 

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

    public class ProjectDetailPromoter : BaseEntity
    {
        public ProjectDetailPromoter()
        {
            DailyPromoterPlans = new List<DailyPromoterPlan>();
        }

        [Display(Name = "پروموتر")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [Display(Name = "فروشگاه - پروژه")]
        public Guid? ProjectDetailId { get; set; }
        public virtual ProjectDetail ProjectDetail { get; set; }

        [Display(Name = "فول تایم است؟")]
        public bool IsFullTime { get; set; }

        public virtual ICollection<DailyPromoterPlan> DailyPromoterPlans { get; set; }
        internal class Configuration : EntityTypeConfiguration<ProjectDetailPromoter>
        {
            public Configuration()
            {
                HasOptional(p => p.ProjectDetail).WithMany(j => j.ProjectDetailPromoters).HasForeignKey(p => p.ProjectDetailId);
                HasRequired(p => p.User).WithMany(j => j.ProjectDetailPromoters).HasForeignKey(p => p.UserId);
            }
        }
    }
}

