using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace codeTongCucThienTai.Models
{
    public class NhanVienModel
    {
        [Required]
        [Display(Name = "Tài khoản")]
        [MaxLength(16, ErrorMessage = "Tài khoản đang dài hơn 16 ký tự!")]
        public string userName { get; set; }

        [Display(Name = "Họ và tên")]
        public string fullName { get; set; }

        [EmailAddress]
        [Display(Name = "Thư điện tử")]
        public string email { get; set; }

        [StringLength(100, ErrorMessage = "Mật khẩu lớn hớn 6 ký tự và nhỏ hơn 100 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không chính xác")]
        public string ConfirmPassword { get; set; }
    }
}