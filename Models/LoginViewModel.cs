using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace desafio.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o e-mail", AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Informe a senha", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Manter Logado!")]
        public bool keepLoggedIn { get; set; }
    }
}