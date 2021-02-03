using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name="کلمه عبور پیشین")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string OldPassword { get; set; }
        [Display(Name="کلمه عبور جدید")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string NewPassword { get; set; }
        [Display(Name="تکرار کلمه عبور جدید")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string RepeatPassword { get; set; }
        public Guid Id { get; set; }
    }
}