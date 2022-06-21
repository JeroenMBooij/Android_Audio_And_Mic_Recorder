using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Models
{
    public class CategoryModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Name_Unique_CSS_ERROR { get; set; }
        public string SoundboardUrl => $"/sound-board/{Id}";
        public List<SoundModel> Sounds { get; set; } = new List<SoundModel>();
    }
}
