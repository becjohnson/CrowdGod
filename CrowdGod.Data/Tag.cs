using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdGod.Data
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string? Text { get; set; }

        public IList<Question>? Questions { get; } = new List<Question>();
    }
}
