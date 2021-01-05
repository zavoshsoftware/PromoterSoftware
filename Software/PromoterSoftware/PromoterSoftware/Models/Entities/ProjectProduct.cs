 

using System.Data.Entity.ModelConfiguration;
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

    public class ProjectProduct : BaseEntity
    {
        public ProjectProduct()
        {
            DailyPromoterProductSales=new List<DailyPromoterProductSale>();
        }

        [Display(Name="پروژه")]
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Display(Name="عنوان محصول")]
        public string ProductTitle { get; set; }

        [Display(Name = "تصویر محصول")]
        public string ImageUrl { get; set; }

        [AllowHtml]
        [Display(Name = "توضیحات محصول")]
        [DataType(DataType.MultilineText)]
        public string ProductDescription { get; set; }

        public virtual ICollection<DailyPromoterProductSale> DailyPromoterProductSales { get; set; }
        internal class Configuration : EntityTypeConfiguration<ProjectProduct>
        {
            public Configuration()
            {
                HasRequired(p => p.Project).WithMany(j => j.ProjectProducts).HasForeignKey(p => p.ProjectId);
            }
        }
    }
}

