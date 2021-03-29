using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class MakeGroupModel
    {
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Nazwa grupy musi mieć minimalnie 6 znaków.")]
        public string Name { get; set; }
    }
}
