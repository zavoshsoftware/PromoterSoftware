 

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class Store : BaseEntity
    {
        public Store()
        {
            ProjectDetails=new List<ProjectDetail>();
        }
        [Display(Name = "عنوان فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Title { get; set; }
         
        [Display(Name = "شهر")]
        public Guid? CityId { get; set; }
      
        public virtual City City { get; set; }

        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
    }
}

