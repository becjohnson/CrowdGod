using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdGod.Data
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Display(Name = "Title:"), MaxLength(50, ErrorMessage = "Please limit your title to 50 characters"), Required(ErrorMessage = "This field is required")]
        public string Title { get; set; }

        [Display(Name = "Question:"), MaxLength(2000, ErrorMessage = "Please limit your question to 2000 characters"), DataType(DataType.MultilineText)]
        public string? Content { get; set; }

        [Display(Name = "Asked on:"), DataType(DataType.Date)]
        public DateTimeOffset CreatedUtc { get; set; }

        [Display(Name = "Edited on:"), DataType(DataType.Date)]
        public DateTimeOffset? ModifiedUtc { get; set; }

        public IList<Tag>? Tags { get; set; } = new List<Tag>();

        public IList<Answer>? Answers { get; } = new List<Answer>();

        public IList<Reply>? Replies { get; } = new List<Reply>(); 
    }
}
