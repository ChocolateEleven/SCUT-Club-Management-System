using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SCUTClubManager.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-z0-9A-Z]{1,15}$",
        ErrorMessage = "密码只能是数字和字母的组合")]
        [MaxLength(15, ErrorMessage = "密码的长度不能超过15个字符")]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^[a-z0-9A-Z]{1,15}$",
        ErrorMessage = "密码只能是数字和字母的组合")]
        [MaxLength(15, ErrorMessage = "密码的长度不能超过15个字符")]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-z0-9A-Z]{1,15}$",
        ErrorMessage = "密码只能是数字和字母的组合")]
        [MaxLength(15, ErrorMessage = "密码的长度不能超过15个字符")]
        [Display(Name = "确认新密码")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "新密码和密码确认不一致")]
        public string ConfirmPassword { get; set; }
    }

    public class LogInModel
    {
        [Required]
        [Display(Name = "用户名")]
        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$",
      ErrorMessage = "用户名只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-z0-9A-Z]{1,15}$",
        ErrorMessage = "密码只能是数字和字母的组合")]
        [MaxLength(15, ErrorMessage = "密码的长度不能超过15个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我的登录？")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "用户名")]
        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$",
      ErrorMessage = "用户名只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-z0-9A-Z]{1,15}$",
        ErrorMessage = "密码只能是数字和字母的组合")]
        [MaxLength(15, ErrorMessage = "密码的长度不能超过15个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [RegularExpression(@"^[a-z0-9A-Z]{1,15}$",
        ErrorMessage = "密码只能是数字和字母的组合")]
        [MaxLength(15, ErrorMessage = "密码的长度不能超过15个字符")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "输入的密码和密码确认不一致")]
        public string ConfirmPassword { get; set; }
    }
}
