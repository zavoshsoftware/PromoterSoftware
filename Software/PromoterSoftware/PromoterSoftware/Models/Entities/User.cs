

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class User : BaseEntity
    {
        public User()
        {
            ProjectDetails = new List<ProjectDetail>();
            UserBankCards = new List<UserBankCard>();
            ProjectDetailPromoters = new List<ProjectDetailPromoter>();
        }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string FullName { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string CellNum { get; set; }

        [Display(Name = "پسورد")]
        [StringLength(150, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Password { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "کارت ملی")]
        public string NationalCardFileUrl { get; set; }

        [Display(Name = "کد کاربر")]
        public int? Code { get; set; }

        [Display(Name = "نقش کاربر")]
        public Guid RoleId { get; set; }

        public virtual Role Role { get; set; }

        [Display(Name = "شهر")]
        public Guid? CityId { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
        public virtual ICollection<UserBankCard> UserBankCards { get; set; }
        public virtual ICollection<ProjectDetailPromoter> ProjectDetailPromoters { get; set; }
    }
}

