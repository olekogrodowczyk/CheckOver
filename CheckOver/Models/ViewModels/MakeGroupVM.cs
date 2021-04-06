using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class MakeGroupVM
    {
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Nazwa grupy musi mieć minimalnie 5 znaków.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Zdjęcie")]
        public IFormFile CoverPhoto { get; set; }

        public string CoverImageUrl { get; set; }
    }
}