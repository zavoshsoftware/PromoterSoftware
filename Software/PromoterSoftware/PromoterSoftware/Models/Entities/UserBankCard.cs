using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class UserBankCard:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Display(Name="شماره کارت")]
        public string CardNumber { get; set; }

        [Display(Name="نام بانک")]
        public string BankTitle { get; set; }

        [Display(Name="شماره شبا")]
        public string ShebaNumber { get; set; }

        [Display(Name="نام صاحب حساب")]
        public string FullName { get; set; }
    }
}