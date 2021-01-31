using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProjectDetailPromoterViewModel
    {
        public List<ProjectDetailPromoter> ProjectDetailPromoters { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        [Display(Name = "ساعت ورود")]
        public string StartHour { get; set; }


        [Display(Name = "ساعت خروج")]
        public string FinishHour { get; set; }

        [Display(Name = "دستمزد ساعتی")]
        public string SalaryPerHour{ get; set; }

        [Display(Name = "هزینه ایاب ذهاب")]
        public string TransportationAmount { get; set; }

        public Guid? UserId { get; set; }
        public Guid  ProjectDetailId { get; set; }
    }
}