using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class ChangeGroupPhotoVM
    {
        [Display(Name = "Zdjęcie")]
        public IFormFile CoverPhoto { get; set; }

        public string CoverImageUrl { get; set; }
    }
}