using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace desafio.Models
{
    public class NewLoginModel
    {
        [Required(ErrorMessage = "O E-mail é obrigatório!", AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória!", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "O tamanho mínimo é de 6 e o máximo é de 20 caracteres!")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Confirmar Senha")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Os campos Senha e Confirmar Senha estão diferentes!")]
        public string ConfirmPassword { get; set; }
    }
}