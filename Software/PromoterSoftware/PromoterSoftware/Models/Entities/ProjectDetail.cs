 

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

    public class ProjectDetail : BaseEntity
    {
        public ProjectDetail()
        {
            ProjectDetailPromoters = new List<ProjectDetailPromoter>();
        }
        [Display(Name="فروشگاه")]
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }

        [Display(Name="ناظر")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [Display(Name="پروژه")]
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Display(Name="ساعت ورود")]
        public decimal StartHour { get; set; }

        [Display(Name="ساعت خروج")]
        public decimal FinishHour { get; set; }

        [Display(Name="دستمزد ساعتی")]
        public decimal SalaryPerHour { get; set; }

        [Display(Name="هزینه ایاب ذهاب")]
        public decimal TransportationAmount { get; set; }

        public virtual ICollection<ProjectDetailPromoter> ProjectDetailPromoters { get; set; }


        internal class Configuration : EntityTypeConfiguration<ProjectDetail>
        {
            public Configuration()
            {
                HasRequired(p => p.Store).WithMany(j => j.ProjectDetails).HasForeignKey(p => p.StoreId);
                HasRequired(p => p.User).WithMany(j => j.ProjectDetails).HasForeignKey(p => p.UserId);
                HasRequired(p => p.Project).WithMany(j => j.ProjectDetails).HasForeignKey(p => p.ProjectId);
            }
        }
    }
}

