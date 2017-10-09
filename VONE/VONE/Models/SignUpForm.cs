using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VONE.Models
{
    public class SignUpForm
    {
        [Required(ErrorMessage = "请输入id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "请输入姓名")]
        public string Account { get; set; }
        [Required(ErrorMessage = "请选择职位")]
        public string Occupation { get; set; }
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(100, ErrorMessage = "请输入长度不小于6的密码", MinimumLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "请输入二次密码")]
        [Compare("Password", ErrorMessage = "确认密码不匹配")]
        public string ConfirmPassword { get; set; }
    }
}