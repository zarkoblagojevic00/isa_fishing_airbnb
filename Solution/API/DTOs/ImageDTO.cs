using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ImageDTO
    {
        [Required(ErrorMessage = "Image needs a service to be assigned to!")]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "The image must have some content!")]
        public string ImageAsBase64 { get; set; }
    }
}
