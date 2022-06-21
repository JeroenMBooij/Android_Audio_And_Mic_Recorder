using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Models
{
    public class SoundModel
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string storageLocation { get; set; }
        public DateTime RecordedAt { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
