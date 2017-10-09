using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VONE.Models
{
    public class GuestResponse
    {
        [Required(ErrorMessage = "请输入账号名")]
        public int Account { get; set; }
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }
        [Required(ErrorMessage = "请选择职位")]
        public string Occupation { get; set; }
    }
}