using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdGod.Data
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        [Display(Name = "Answer:"), Required(ErrorMessage = "This field is required"), MaxLength(2000, ErrorMessage = "Please limit your answer to 2000 characters"), DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Asked on:"), DataType(DataType.Date)]
        public DateTimeOffset CreatedUtc { get; set; }

        [Display(Name = "Edited on:"), DataType(DataType.Date)]
        public DateTimeOffset? ModifiedUtc { get; set; }

        public int? QuestionId { get; set; }
        public Question? Question { get; set; }

        public IList<Reply>? Replies { get; } = new List<Reply>();
    }
}
