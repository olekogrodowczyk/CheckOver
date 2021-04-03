using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class GroupSettings
    {
        public string Name { get; set; }
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
